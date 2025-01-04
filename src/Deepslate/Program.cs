using Deepslate.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Photino.NET;
using Photino.NET.Server;
using System.Drawing;
using System.Text;

namespace Photino.HelloPhotino.React;
//NOTE: To hide the console window, go to the project properties and change the Output Type to Windows Application.
// Or edit the .csproj file and change the <OutputType> tag from "WinExe" to "Exe".

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        PhotinoServer
            .CreateStaticFileServer(args, out string baseUrl)
            .RunAsync();

        Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHostedService<WindowHostedService>();
                services.AddSingleton<CommunicationService>();
                services.AddSingleton<PhotinoWindow>(x =>
                {
                    return new PhotinoWindow()
                    .SetTitle("Deepslate Launcher")
                    .SetUseOsDefaultSize(false)
                    .SetSize(new Size(2048, 1024))
                    // Resize to a percentage of the main monitor work area
                    //.Resize(50, 50, "%")
                    .SetUseOsDefaultSize(false)
                    .SetSize(new Size(800, 600))
                    // Center window in the middle of the screen
                    .Center()
                    // Users can resize windows by default.
                    // Let's make this one fixed instead.
                    .SetResizable(true)
                    .RegisterCustomSchemeHandler("app", (object sender, string scheme, string url, out string contentType) =>
                    {
                        contentType = "text/javascript";
                        return new MemoryStream(Encoding.UTF8.GetBytes(@"
                                (() =>{
                                    window.setTimeout(() => {
                                        alert(`🎉 Dynamically inserted JavaScript.`);
                                    }, 1000);
                                })();
                            "));
                    })
                    // Most event handlers can be registered after the
                    // PhotinoWindow was instantiated by calling a registration 
                    // method like the following RegisterWebMessageReceivedHandler.
                    // This could be added in the PhotinoWindowOptions if preferred.
                    .RegisterWebMessageReceivedHandler((sender, message) =>
                    {
                        x.GetRequiredService<CommunicationService>().ReceiveFromFrontend(message);
                    })
                    .Load($"{baseUrl}/index.html"); // Can be used with relative path strings or "new URI()" instance to load a website.
                });
            })
            .Build()
            .RunAsync();
    }
}
