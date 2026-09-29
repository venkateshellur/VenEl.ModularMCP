using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.AWS.Extensions;

namespace VenEl.ModularMCP.AWS
{
    public class AWSPlugin : IVenElPlugin
    {
        public string Name => "AWS";
        public string Description => "Provides AWS tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAwsFeature(configuration);
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[AWS] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
