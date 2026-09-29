using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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
                        if ((DateTime.UtcNow - lastCheck).TotalHours < 24)
                        {
                            return; // Less than 24 hours ago, skip update
                        }
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
                Console.Error.WriteLine($"[AutoUpdate] Failed to run daily update: {ex.Message}");
            }
        }

        public static async Task InstallOrUpdatePluginAsync(string pluginName, string pluginDir, bool silent = false)
        {
            try
            {
                var packageId = $"VenEl.ModularMCP.{pluginName}".ToLowerInvariant();
                var indexUrl = $"https://api.nuget.org/v3-flatcontainer/{packageId}/index.json";

                HttpResponseMessage indexResponse = await _httpClient.GetAsync(indexUrl);
                if (!indexResponse.IsSuccessStatusCode)
                {
                    if (!silent)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Error.WriteLine($"[Error] Could not find plugin '{pluginName}' on NuGet.");
                        Console.ResetColor();
                    }
                    return;
                }

                var indexJson = await indexResponse.Content.ReadAsStringAsync();
                var indexDoc = JsonDocument.Parse(indexJson);
                
                var versions = indexDoc.RootElement.GetProperty("versions").EnumerateArray().Select(v => v.GetString()).ToList();
                if (versions.Count == 0) return;

                var latestVersion = versions.Last(); // NuGet flatcontainer returns sorted versions

                if (!silent)
                {
                    Console.Error.WriteLine($"Installing {pluginName} v{latestVersion} from NuGet...");
                }

                var downloadUrl = $"https://api.nuget.org/v3-flatcontainer/{packageId}/{latestVersion}/{packageId}.{latestVersion}.nupkg";
                
                var tempFile = Path.GetTempFileName();
                using (var s = await _httpClient.GetStreamAsync(downloadUrl))
                using (var fs = new FileStream(tempFile, FileMode.OpenOrCreate))
                {
                    await s.CopyToAsync(fs);
                }

                if (!Directory.Exists(pluginDir))
                {
                    Directory.CreateDirectory(pluginDir);
                }

                var targetDir = Path.Combine(pluginDir, pluginName);
                if (Directory.Exists(targetDir)) 
                {
                    Directory.Delete(targetDir, true);
                }
                Directory.CreateDirectory(targetDir);

                // Open the .nupkg (which is just a zip archive)
                using (var archive = ZipFile.OpenRead(tempFile))
                {
                    foreach (var entry in archive.Entries)
                    {
                        // We only want to extract the DLLs from the lib/net8.0/ folder
                        if (entry.FullName.StartsWith("lib/net8.0/") && !string.IsNullOrEmpty(entry.Name))
                        {
                            var destinationPath = Path.Combine(targetDir, entry.Name);
                            entry.ExtractToFile(destinationPath, true);
                            if (!silent) Console.Error.WriteLine($"  -> Extracted: {entry.Name}");
                        }
                    }
                }

                File.Delete(tempFile);

                if (!silent)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Error.WriteLine($"\n[Success] Successfully installed '{pluginName}' v{latestVersion} into {targetDir}");
                    Console.Error.WriteLine("It will be dynamically loaded the next time you boot the Core host.");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine($"[Error] Failed to install plugin '{pluginName}': {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }
}
