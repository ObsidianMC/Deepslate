using Deepslate.LauncherLib;
using System.Diagnostics;
using System.Reflection;

namespace Deepslate.Commandline
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var gameDownloader = new GameDownloader();

            var versions = await gameDownloader.ListAvailableVersionsAsync();

            Console.WriteLine("Available versions:");
            foreach (var version in versions)
            {
                Console.WriteLine(version);
            }

            await gameDownloader.DownloadGameAsync(versions.First(), versions.First());
        }
    }
}
