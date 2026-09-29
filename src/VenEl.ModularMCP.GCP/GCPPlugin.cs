using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.GCP.Extensions;

namespace VenEl.ModularMCP.GCP
{
    public class GCPPlugin : IVenElPlugin
    {
        public string Name => "GCP";
        public string Description => "Provides GCP tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registration for GCP goes here
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[GCP] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
