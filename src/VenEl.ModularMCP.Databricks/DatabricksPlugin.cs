using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.Databricks.Extensions;

namespace VenEl.ModularMCP.Databricks
{
    public class DatabricksPlugin : IVenElPlugin
    {
        public string Name => "Databricks";
        public string Description => "Provides Databricks tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registration for Databricks goes here
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[Databricks] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
