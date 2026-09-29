using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VenEl.ModularMCP.Shared;
using VenEl.MCP.FTP.Extensions;

namespace VenEl.ModularMCP.FTP
{
    public class FTPPlugin : IVenElPlugin
    {
        public string Name => "FTP";
        public string Description => "Provides FTP tools.";

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registration for FTP goes here
        }

        public Task InitializeAsync()
        {
            Console.Error.WriteLine("[FTP] Initialized dynamically!");
            return Task.CompletedTask;
        }
    }
}
