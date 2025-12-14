using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.Launcher;

/// <summary>
/// Asset updater that fetches and extracts asset data from the UGC or latest data repository
/// </summary>
public class AssetUpdater
{
    private const string RepoOwner = "hereelabs";
    private const string RepoName = "ORRH-UGC-Repository";
    private const string Branch = "main";
    private const string VersionFileName = "assets.version";

    // So we arent downloading a bunch of crap we dont need, only these directories will be extracted
    private static readonly string[] RequiredDirectories = { "data/", "maps/" };

    /// <summary>
    /// Async method to update assets by checking the latest commit hash and downloading new assets if needed
    /// </summary>
    /// <param name="launcherBaseDirectory">The directory where the base assets are stored</param>
    /// <returns>Nothing, will exit once completed or failed</returns>
    public async static Task UpdateAssetsAsync(string launcherBaseDirectory)
    {
        if (Settings.Default.Launch.DoUpdateAssets == false)
        {
            Logger.Instance.Info("asset updating is disabled in settings...checking asset freshness");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ORRH-Launcher", "1.0"));

                try
                {
                    string latestHash = await GetLatestCommitHashAsync(client);
                    string localHashPath = Path.Combine(launcherBaseDirectory, VersionFileName);
                    string currentHash = File.Exists(localHashPath) ? File.ReadAllText(localHashPath).Trim() : "";
                    string shortHash = latestHash.Substring(0, 7);

                    if (string.Equals(latestHash, currentHash, StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.Instance.Info($"assets are up to date. ({shortHash}).");
                        Settings.Default.Launch.OudatedAssets = false;
                        return;
                    }

                    Logger.Instance.Info($"newer assets detected! ({shortHash}).");
                    Settings.Default.Launch.OudatedAssets = true;
                    Settings.Default.Launch.LatestHash = shortHash;
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Instance.Info($"failed to check asset freshness: {ex.Message}");
                    return;
                }
            }

        }
        Logger.Instance.Info("checking for new asset data...");

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ORRH-Launcher", "1.0"));

            try
            {
                string latestHash = await GetLatestCommitHashAsync(client);
                string localHashPath = Path.Combine(launcherBaseDirectory, VersionFileName);
                string currentHash = File.Exists(localHashPath) ? File.ReadAllText(localHashPath).Trim() : "";
                string shortHash = latestHash.Substring(0, 7);

                if (string.Equals(latestHash, currentHash, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Instance.Info($"assets are up to date. ({shortHash}).");
                    Settings.Default.Launch.OudatedAssets = false;
                    return;
                }

                Logger.Instance.Info($"newer assets detected! ({shortHash}). downloading...");

                string zipUrl = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/zipball/{Branch}";

                using (var response = await client.GetAsync(zipUrl, HttpCompletionOption.ResponseHeadersRead))
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    ExtractWhitelistedAssets(archive, launcherBaseDirectory);
                }

                File.WriteAllText(localHashPath, latestHash);
                Logger.Instance.Info($"assets updated successfully to version {shortHash}.");
                string message = $"Your assets have been updated successfully to version {shortHash}!";
                Settings.Default.Launch.OudatedAssets = false;
                Utils.ShowMessageBox(message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.Instance.Info($"failed to update assets: {ex.Message}");
                Utils.ShowMessageBox("We tried to update your assets, but something went wrong. (Maybe you hit the request limit?)", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
                return;
            }
        }
    }

    private static async Task<string> GetLatestCommitHashAsync(HttpClient client)
    {
        string url = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/commits/{Branch}";
        var json = await client.GetStringAsync(url);

        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            return doc.RootElement.GetProperty("sha").GetString();
        }
    }

    private static void ExtractWhitelistedAssets(ZipArchive archive, string baseDir)
    {
        foreach (var entry in archive.Entries)
        {
            int firstSlashIndex = entry.FullName.IndexOf('/');
            if (firstSlashIndex < 0) continue;

            string relativePath = entry.FullName.Substring(firstSlashIndex + 1);

            if (string.IsNullOrWhiteSpace(relativePath)) continue;

            if (RequiredDirectories.Any(dir => relativePath.StartsWith(dir, StringComparison.OrdinalIgnoreCase)))
            {
                string destinationPath = Path.Combine(baseDir, relativePath);
                string destinationDir = Path.GetDirectoryName(destinationPath);

                if (!Directory.Exists(destinationDir))
                    Directory.CreateDirectory(destinationDir);

                if (!string.IsNullOrEmpty(Path.GetFileName(destinationPath)))
                {
                    Logger.Instance.Info($"extracting assets: {relativePath}");
                    entry.ExtractToFile(destinationPath, overwrite: true);
                }
            }
        }
    }
}