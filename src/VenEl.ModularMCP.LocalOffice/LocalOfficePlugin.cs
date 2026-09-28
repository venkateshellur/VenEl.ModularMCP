using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.LocalOffice.Extensions;

namespace VenEl.ModularMCP.LocalOffice
{
    public class LocalOfficePlugin : IVenElPlugin
    {
        public string Name => "LocalOffice";
        public string Description => "Provides LocalOffice tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registration for LocalOffice goes here
        }

        public Task InitializeAsync()
        {
            Console.WriteLine("[LocalOffice] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
