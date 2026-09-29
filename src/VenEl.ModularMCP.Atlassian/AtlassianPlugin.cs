using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.Atlassian.Extensions;

namespace VenEl.ModularMCP.Atlassian
{
    public class AtlassianPlugin : IVenElPlugin
    {
        public string Name => "Atlassian";
        public string Description => "Provides Atlassian tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registration for Atlassian goes here
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[Atlassian] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
