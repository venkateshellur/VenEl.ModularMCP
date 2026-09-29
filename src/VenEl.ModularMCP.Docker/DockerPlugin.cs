using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.Docker.Extensions;

namespace VenEl.ModularMCP.Docker
{
    public class DockerPlugin : IVenElPlugin
    {
        public string Name => "Docker";
        public string Description => "Provides Docker tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registration for Docker goes here
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[Docker] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
