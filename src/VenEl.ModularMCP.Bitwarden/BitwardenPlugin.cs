using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.Bitwarden.Extensions;

namespace VenEl.ModularMCP.Bitwarden
{
    public class BitwardenPlugin : IVenElPlugin
    {
        public string Name => "Bitwarden";
        public string Description => "Provides Bitwarden tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registration for Bitwarden goes here
        }

        public Task InitializeAsync()
        {
            Console.WriteLine("[Bitwarden] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
