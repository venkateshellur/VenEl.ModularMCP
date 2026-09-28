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
            // Registration for Email goes here
        }

        public Task InitializeAsync()
        {
            Console.WriteLine("[Email] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
