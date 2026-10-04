using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.ServiceNow.Extensions;

namespace VenEl.ModularMCP.ServiceNow
{
    public class ServiceNowPlugin : IVenElPlugin
    {
        public string Name => "ServiceNow";
        public string Description => "Provides ServiceNow API tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddServiceNowFeature(configuration);
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[ServiceNow] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
