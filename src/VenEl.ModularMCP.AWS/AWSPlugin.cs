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
            // Registration for AWS goes here
        }

        public Task InitializeAsync()
        {
            Console.WriteLine("[AWS] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
