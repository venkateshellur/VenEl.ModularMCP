using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace VenEl.ModularMCP.Shared
{
    /// <summary>
    /// Represents the core contract that all VenEl MCP plugins must implement.
    /// The Core server will use AssemblyLoadContext to find and instantiate classes implementing this interface.
    /// </summary>
    public interface IVenElPlugin
    {
        /// <summary>
        /// The name of the plugin (e.g., "Atlassian", "ServiceNow").
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A short description of what this plugin does.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Allows the plugin to register its own services.
        /// </summary>
        void ConfigureServices(IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration);

        /// <summary>
        /// Allows the plugin to execute startup logic.
        /// </summary>
        Task InitializeAsync();
    }
}
