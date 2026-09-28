using System;

namespace VenEl.ModularMCP.Shared
{
    public static class PluginExecutableBase
    {
        public static void Run(string pluginName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️ Notice: {pluginName} is built as an extension to VenEl.ModularMCP.");
            Console.WriteLine("It is a dynamically loaded plugin and cannot be run directly as a standalone executable.");
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("To use these tools, please install the core host first:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("    dotnet tool install -g VenEl.ModularMCP");
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("And then install this plugin via the core CLI:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    venel-mcp plugin install {pluginName}");
            Console.ResetColor();
        }
    }
}
