using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VenEl.MCP.Core.Registration;
using VenEl.MCP.Core.Extensions;
using VenEl.ModularMCP.Shared;

namespace VenEl.ModularMCP.Core
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var userConfigDir = Path.Combine(userProfile, ".venel-modular-mcp");
            var pluginDir = Path.Combine(userConfigDir, "plugins");

            // CLI Command Parsing for Plugin Installation
            if (args.Length >= 3 && args[0].Equals("plugin", StringComparison.OrdinalIgnoreCase) && args[1].Equals("install", StringComparison.OrdinalIgnoreCase))
            {
                var pluginName = args[2];
                await PluginUpdater.InstallOrUpdatePluginAsync(pluginName, pluginDir);
                return;
            }

            Console.Error.WriteLine("Starting VenEl.ModularMCP Core Host...");

            // Daily Auto-Update (Runs synchronously on boot before DLLs are locked in memory)
            await PluginUpdater.RunDailyAutoUpdateAsync(pluginDir);

            var pluginManager = new PluginManager();
            
            Console.Error.WriteLine($"Scanning for dynamic plugins in: {pluginDir}");
            var plugins = pluginManager.LoadPlugins(pluginDir);
            Console.Error.WriteLine($"Engine loaded {plugins.Count} plugins successfully.");

            var builder = Host.CreateEmptyApplicationBuilder(settings: null);

            var userConfigPath = Path.Combine(userConfigDir, "appsettings.json");

            builder.Configuration
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile(userConfigPath, optional: true, reloadOnChange: true)
                .AddEnvironmentVariables(prefix: "VENEL_");

            builder.Logging
                .ClearProviders()
                .SetMinimumLevel(LogLevel.Warning);

            var pluginsToRemove = new System.Collections.Generic.List<IVenElPlugin>();
            foreach (var plugin in plugins)
            {
                try
                {
                    Console.Error.WriteLine($"Configuring Services for: {plugin.Name}");
                    plugin.ConfigureServices(builder.Services, builder.Configuration);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"[Core] {plugin.Name} threw an error during ConfigureServices: {ex.Message}");
                    Console.Error.WriteLine($"[Core] Attempting self-healing update for {plugin.Name}...");
                    Console.ResetColor();

                    var success = PluginUpdater.InstallOrUpdatePluginAsync(plugin.Name, pluginDir).GetAwaiter().GetResult();
                    if (success)
                    {
                        Console.Error.WriteLine($"[Core] Self-healing complete. Please restart the MCP engine to apply the newly updated {plugin.Name} plugin.");
                    }
                    else
                    {
                        Console.Error.WriteLine($"[Core] Self-healing failed. Disabling {plugin.Name} for this session.");
                    }
                    pluginsToRemove.Add(plugin);
                }
            }

            foreach (var p in pluginsToRemove)
            {
                plugins.Remove(p);
            }

            builder.Services.AddCoreSecurity();

            var mcpBuilder = builder.Services
                .AddMcpServer(options =>
                {
                    options.ServerInfo = new()
                    {
                        Name = "VenEl.ModularMCP",
                        Version = "1.0.14" // Bumped version
                    };
                })
                .WithStdioServerTransport();

                        var registry = builder.Services.GetOrAddFeatureRegistry();
            Console.Error.WriteLine($"Total Registered Features: {registry.Registrations.Count}");
            registry.ApplyAll(mcpBuilder, null);

            Console.Error.WriteLine("Dependency Injection container & MCP Server built successfully.");

            pluginsToRemove.Clear();
            foreach (var plugin in plugins)
            {
                try
                {
                    Console.Error.WriteLine($"Initializing: {plugin.Name}");
                    await plugin.InitializeAsync();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"[Core] {plugin.Name} threw an error during InitializeAsync: {ex.Message}");
                    Console.Error.WriteLine($"[Core] Attempting self-healing update for {plugin.Name}...");
                    Console.ResetColor();

                    var success = await PluginUpdater.InstallOrUpdatePluginAsync(plugin.Name, pluginDir);
                    if (success)
                    {
                        Console.Error.WriteLine($"[Core] Self-healing complete. Please restart the MCP engine to apply the newly updated {plugin.Name} plugin.");
                    }
                    else
                    {
                        Console.Error.WriteLine($"[Core] Self-healing failed. Disabling {plugin.Name} for this session.");
                    }
                    pluginsToRemove.Add(plugin);
                }
            }

            foreach (var p in pluginsToRemove)
            {
                plugins.Remove(p);
            }

            Console.Error.WriteLine("Core is running and waiting for MCP connections...");
            
            var host = builder.Build();
            await host.RunAsync();
        }
    }
}
