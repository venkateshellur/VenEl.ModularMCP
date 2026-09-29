using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.GitHub.Extensions;

namespace VenEl.ModularMCP.GitHub
{
    public class GitHubPlugin : IVenElPlugin
    {
        public string Name => "GitHub";
        public string Description => "Provides GitHub tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddGitHubFeature(configuration);
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[GitHub] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
