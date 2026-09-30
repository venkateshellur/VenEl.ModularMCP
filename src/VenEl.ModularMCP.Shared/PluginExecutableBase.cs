using System;

namespace VenEl.ModularMCP.Shared
{
    public static class PluginExecutableBase
    {
        public static void Run(string pluginName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Error.WriteLine($"⚠️ Notice: {pluginName} is built as an extension to VenEl.ModularMCP.");
            Console.Error.WriteLine("It is a dynamically loaded plugin and cannot be run directly as a standalone executable.");
            Console.Error.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Error.WriteLine("To use these tools, please install the core host first:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Error.WriteLine("    dotnet tool install -g VenEl.ModularMCP");
            Console.Error.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Error.WriteLine("And then install this plugin via the core CLI:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Error.WriteLine($"    venel-modular-mcp plugin install {pluginName}");
            Console.ResetColor();
        }
    }
}
