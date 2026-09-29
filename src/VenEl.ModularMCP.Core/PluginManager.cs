using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using VenEl.ModularMCP.Shared;

namespace VenEl.ModularMCP.Core
{
    // 1. Custom Load Context (Allows for DLL isolation and future unloading)
    public class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            // Delegate shared assemblies to the default context to prevent type mismatch
            if (assemblyName.Name == "VenEl.ModularMCP.Shared" ||
                assemblyName.Name == "Microsoft.Extensions.DependencyInjection.Abstractions" ||
                assemblyName.Name == "Microsoft.Extensions.Configuration.Abstractions")
            {
                return null;
            }

            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
            return IntPtr.Zero;
        }
    }

    // 2. The Plugin Manager that scans the directory and instantiates IVenElPlugin
    public class PluginManager
    {
        public List<IVenElPlugin> LoadPlugins(string pluginsDirectory)
        {
            var plugins = new List<IVenElPlugin>();

            if (!Directory.Exists(pluginsDirectory))
            {
                Directory.CreateDirectory(pluginsDirectory);
                Console.Error.WriteLine($"[PluginManager] Created empty plugins directory at: {pluginsDirectory}");
                return plugins;
            }

            // Find all DLLs in the plugins directory that match the plugin naming convention
            var pluginDlls = Directory.GetFiles(pluginsDirectory, "VenEl.ModularMCP.*.dll", SearchOption.AllDirectories);

            foreach (var dllPath in pluginDlls)
            {
                try
                {
                    var fileName = Path.GetFileName(dllPath);
                    // Skip the shared interface DLL and Core DLL to prevent type mismatch errors
                    if (fileName.Equals("VenEl.ModularMCP.Shared.dll", StringComparison.OrdinalIgnoreCase) || 
                        fileName.Equals("VenEl.ModularMCP.Core.dll", StringComparison.OrdinalIgnoreCase))
                        continue;

                    var loadContext = new PluginLoadContext(dllPath);
                    var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(dllPath)));

                    // Scan the assembly for classes implementing IVenElPlugin
                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(IVenElPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            var plugin = Activator.CreateInstance(type) as IVenElPlugin;
                            if (plugin != null)
                            {
                                plugins.Add(plugin);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Error.WriteLine($"[PluginManager] Successfully Loaded: {plugin.Name}");
                                Console.ResetColor();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"[PluginManager] Failed to load {Path.GetFileName(dllPath)}. Reason: {ex.Message}");
                    Console.ResetColor();
                }
            }

            return plugins;
        }
    }
}
