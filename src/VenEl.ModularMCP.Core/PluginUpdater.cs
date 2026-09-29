using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace VenEl.ModularMCP.Core
{
    public static class PluginUpdater
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task RunDailyAutoUpdateAsync(string pluginDir)
        {
            try
            {
                if (!Directory.Exists(pluginDir)) return;
                var lastCheckFile = Path.Combine(pluginDir, ".last_update_check");
                if (File.Exists(lastCheckFile))
                {
                    if (DateTime.TryParse(await File.ReadAllTextAsync(lastCheckFile), out DateTime lastCheck))
                    {
                        if ((DateTime.UtcNow - lastCheck).TotalHours < 24) return;
                    }
                }

                Console.Error.WriteLine("[AutoUpdate] Checking NuGet for plugin updates...");
                var installedPlugins = Directory.GetDirectories(pluginDir);
                foreach (var pluginPath in installedPlugins)
                {
                    var pluginName = Path.GetFileName(pluginPath);
                    await InstallOrUpdatePluginAsync(pluginName, pluginDir, silent: true);
                }
                await File.WriteAllTextAsync(lastCheckFile, DateTime.UtcNow.ToString("O"));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[AutoUpdate] Failed: {ex.Message}");
            }
        }

        public static async Task InstallOrUpdatePluginAsync(string pluginName, string pluginDir, bool silent = false)
        {
            try
            {
                var packageId = $"VenEl.ModularMCP.{pluginName}".ToLowerInvariant();
                var indexUrl = $"https://api.nuget.org/v3-flatcontainer/{packageId}/index.json";

                var indexResponse = await _httpClient.GetAsync(indexUrl);
                if (!indexResponse.IsSuccessStatusCode)
                {
                    if (!silent) { Console.ForegroundColor = ConsoleColor.Red; Console.Error.WriteLine($"[Error] Could not find plugin '{pluginName}' on NuGet."); Console.ResetColor(); }
                    return;
                }

                var indexDoc = JsonDocument.Parse(await indexResponse.Content.ReadAsStringAsync());
                var versions = indexDoc.RootElement.GetProperty("versions").EnumerateArray().Select(v => v.GetString()).ToList();
                if (versions.Count == 0) return;
                var latestVersion = versions.Last();

                if (!silent) Console.Error.WriteLine($"Resolving dependencies for {pluginName} v{latestVersion} via NuGet...");

                if (!Directory.Exists(pluginDir)) Directory.CreateDirectory(pluginDir);
                var targetDir = Path.Combine(pluginDir, pluginName);

                // Use the MSBuild Dummy Project trick to fetch the plugin AND all its transient dependencies
                var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempDir);
                var csprojPath = Path.Combine(tempDir, "PluginInstall.csproj");
                
                var csprojContent = $@"
<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
    <!-- Prevent it from building an executable, we just want to resolve the library -->
    <OutputType>Library</OutputType> 
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""{packageId}"" Version=""{latestVersion}"" />
  </ItemGroup>
</Project>";
                await File.WriteAllTextAsync(csprojPath, csprojContent);

                var processInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"publish \"{csprojPath}\" -c Release -o \"{targetDir}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    await process.WaitForExitAsync();
                    if (process.ExitCode != 0)
                    {
                        var error = await process.StandardError.ReadToEndAsync();
                        throw new Exception($"dotnet publish failed: {error}");
                    }
                }

                // Cleanup temp dir
                Directory.Delete(tempDir, true);

                if (!silent)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Error.WriteLine($"\n[Success] Successfully installed '{pluginName}' and ALL dependencies into {targetDir}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                if (!silent) { Console.ForegroundColor = ConsoleColor.Red; Console.Error.WriteLine($"[Error] Failed to install plugin '{pluginName}': {ex.Message}"); Console.ResetColor(); }
            }
        }
    }
}
