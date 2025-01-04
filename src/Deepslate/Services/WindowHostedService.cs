using Microsoft.Extensions.Hosting;
using Photino.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepslate.Services
{
    public class WindowHostedService : IHostedService
    {
        private readonly PhotinoWindow _window;
        public WindowHostedService(PhotinoWindow window)
        {
            _window = window;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _window.WaitForClose();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _window.Close();
            return Task.CompletedTask;
        }
    }
}
