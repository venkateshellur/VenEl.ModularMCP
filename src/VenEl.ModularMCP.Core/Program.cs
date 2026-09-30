using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VenEl.MCP.Core.Registration;
using VenEl.MCP.Core.Extensions;

namespace VenEl.ModularMCP.Core
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var userConfigDir = Path.Combine(userProfile, ".venel-mcp");
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

            foreach (var plugin in plugins)
            {
                Console.Error.WriteLine($"Configuring Services for: {plugin.Name}");
                plugin.ConfigureServices(builder.Services, builder.Configuration);
            }

            builder.Services.AddCoreSecurity();

            var mcpBuilder = builder.Services
                .AddMcpServer(options =>
                {
                    options.ServerInfo = new()
                    {
                        Name = "VenEl.ModularMCP",
                        Version = "1.0.10" // Bumped version
                    };
                })
                .WithStdioServerTransport();

                        var registry = builder.Services.GetOrAddFeatureRegistry();
            Console.Error.WriteLine($"Total Registered Features: {registry.Registrations.Count}");
            registry.ApplyAll(mcpBuilder, null);

            Console.Error.WriteLine("Dependency Injection container & MCP Server built successfully.");

            foreach (var plugin in plugins)
            {
                Console.Error.WriteLine($"Initializing: {plugin.Name}");
                await plugin.InitializeAsync();
            }

            Console.Error.WriteLine("Core is running and waiting for MCP connections...");
            
            var host = builder.Build();
            await host.RunAsync();
        }
    }
}
