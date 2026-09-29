using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.MicrosoftTeams.Extensions;

namespace VenEl.ModularMCP.MicrosoftTeams
{
    public class MicrosoftTeamsPlugin : IVenElPlugin
    {
        public string Name => "MicrosoftTeams";
        public string Description => "Provides MicrosoftTeams tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registration for MicrosoftTeams goes here
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[MicrosoftTeams] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
