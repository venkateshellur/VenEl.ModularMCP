using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.Slack.Extensions;

namespace VenEl.ModularMCP.Slack
{
    public class SlackPlugin : IVenElPlugin
    {
        public string Name => "Slack";
        public string Description => "Provides Slack tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registration for Slack goes here
        }

        public Task InitializeAsync()
        {
            Console.WriteLine("[Slack] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
