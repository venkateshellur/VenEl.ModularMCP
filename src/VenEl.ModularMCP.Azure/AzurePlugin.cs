using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.Azure.Extensions;

namespace VenEl.ModularMCP.Azure
{
    public class AzurePlugin : IVenElPlugin
    {
        public string Name => "Azure";
        public string Description => "Provides Azure tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureFeature(configuration);
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[Azure] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
