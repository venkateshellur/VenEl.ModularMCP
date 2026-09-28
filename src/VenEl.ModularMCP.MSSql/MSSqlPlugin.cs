using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.MSSql.Extensions;

namespace VenEl.ModularMCP.MSSql
{
    public class MSSqlPlugin : IVenElPlugin
    {
        public string Name => "MSSql";
        public string Description => "Provides MSSql tools.";

        public void ConfigureServices(IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddMSSqlFeature(configuration);
        }

        public Task InitializeAsync()
        {
            Console.WriteLine("[MSSql] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
