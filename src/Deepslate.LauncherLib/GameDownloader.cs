using Deepslate.LauncherLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Deepslate.LauncherLib
{
    public class GameDownloader
    {
        const string MINECRAFT_VERSION_MANIFEST_URL = "https://launchermeta.mojang.com/mc/game/version_manifest_v2.json";
        const string MINECRAFT_JAVA_VERSION_MANIFEST_URL = "https://launchermeta.mojang.com/v1/products/java-runtime/2ec0cc96c44e5a76b9c8b7c39df7210883d12871/all.json";

        private HttpClient _httpClient;
        private string osType;

        public GameDownloader()
        {
            _httpClient = new HttpClient();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                if(Environment.Is64BitOperatingSystem)
                    osType = "windows-x64";
                else
                    osType = "windows-x86";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                if(Environment.Is64BitOperatingSystem)
                    osType = "linux";
                else
                    osType = "linux-i386";
            // TODO this might need work, I don't have a mac
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                if (Environment.Is64BitOperatingSystem)
                    osType = "mac-os-arm64";
                else
                    osType = "mac-os";
            else
                throw new PlatformNotSupportedException();
        }

        public async ValueTask<bool> DownloadGameAsync(string path, string gameVersion)
        {
            createDirectories(path);

            var manifest = await GetJsonAsync<VersionManifest>(MINECRAFT_VERSION_MANIFEST_URL);
            var version = manifest.Versions.First(v => v.Id == gameVersion);
            var versionMetadata = await GetJsonAsync<VersionMetadata>(version.GameVersionManifestUrl);

            var javaVersion = versionMetadata.JavaVersion.Component;
            var javaVersions = await GetJsonAsync<JsonObject>(MINECRAFT_JAVA_VERSION_MANIFEST_URL);

            var javaDownloadManifestUrl = javaVersions[osType][javaVersion][0]["manifest"]["url"].ToString();
            await DownloadJavaAsync(javaDownloadManifestUrl, path);

            return true;
        }

        public async ValueTask<IEnumerable<string>> ListAvailableVersionsAsync()
        {
            var manifest = await GetJsonAsync<VersionManifest>(MINECRAFT_VERSION_MANIFEST_URL);
            return manifest.Versions.Select(v => v.Id);
        }

        private void createDirectories(string path)
        {
            Directory.CreateDirectory(Path.Combine(path, "assets"));
            Directory.CreateDirectory(Path.Combine(path, "libraries"));
            Directory.CreateDirectory(Path.Combine(path, "gamefiles"));
            Directory.CreateDirectory(Path.Combine(path, "game"));
            Directory.CreateDirectory(Path.Combine(path, "java"));
        }

        private async Task DownloadJavaAsync(string url, string basePath)
        {
            var javaFiles = (await GetJsonAsync<JsonObject>(url))["files"];
            var javaDir = Path.Combine(basePath, "java");
            foreach (var file in javaFiles.AsObject())
            {
                var fileValue = file.Value;
                var fileName = file.Key;

                if (fileValue["type"].ToString() == "directory")
                {
                    Directory.CreateDirectory(Path.Combine(javaDir, fileName));
                }
                else
                {
                    var downloadUrl = fileValue["downloads"]["raw"]["url"].ToString();
                    var downloadPath = Path.Combine(javaDir, fileName);
                    if(File.Exists(downloadPath))
                        continue;
                    using (var stream = await DownloadAsync(downloadUrl))
                    using (var fileStream = File.Create(downloadPath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
        }

        private async ValueTask<Stream> DownloadAssetAsync(string hash)
            => await DownloadAsync($"https://resources.download.minecraft.net/{hash.Substring(0, 2)}/{hash}");

        private async ValueTask<Stream> DownloadAsync(string url)
            => await _httpClient.GetStreamAsync(url);

        private async ValueTask<T> GetJsonAsync<T>(string url)
            => JsonSerializer.Deserialize<T>(await _httpClient.GetStreamAsync(url));
    }
}
