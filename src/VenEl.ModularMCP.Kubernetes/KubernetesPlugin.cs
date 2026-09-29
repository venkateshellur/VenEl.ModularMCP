using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.Kubernetes.Extensions;

namespace VenEl.ModularMCP.Kubernetes
{
    public class KubernetesPlugin : IVenElPlugin
    {
        public string Name => "Kubernetes";
        public string Description => "Provides Kubernetes tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddKubernetesFeature(configuration);
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[Kubernetes] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
