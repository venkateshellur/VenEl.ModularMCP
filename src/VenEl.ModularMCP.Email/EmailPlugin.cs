using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.Email.Extensions;

namespace VenEl.ModularMCP.Email
{
    public class EmailPlugin : IVenElPlugin
    {
        public string Name => "Email";
        public string Description => "Provides Email tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddEmailFeature(configuration);
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[Email] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
