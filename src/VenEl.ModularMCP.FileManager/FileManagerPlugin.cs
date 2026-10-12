using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.FileManager.Extensions;

namespace VenEl.ModularMCP.FileManager
{
    public class FileManagerPlugin : IVenElPlugin
    {
        public string Name => "FileManager";
        public string Description => "Provides interactive File Explorer and filesystem MCP tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddFileManagerFeature(configuration);
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[FileManager] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
