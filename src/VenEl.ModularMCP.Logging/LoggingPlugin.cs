using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.Logging.Extensions;

namespace VenEl.ModularMCP.Logging
{
    public class LoggingPlugin : IVenElPlugin
    {
        public string Name => "Logging";
        public string Description => "Provides Logging tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLoggingFeature(configuration);
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[Logging] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
