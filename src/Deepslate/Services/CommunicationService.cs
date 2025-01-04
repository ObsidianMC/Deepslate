using Deepslate.Payloads;
using Microsoft.Extensions.Logging;
using Photino.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Deepslate.Services
{
    public class CommunicationService
    {
        private readonly PhotinoWindow _window;
        private readonly ILogger _logger;

        public CommunicationService(PhotinoWindow window, ILogger<CommunicationService> logger)
        {
            _window = window;
            _logger = logger;
        }

        public void SendToFrontend(string message)
        {
            _window.SendWebMessage(message);
        }

        public void ReceiveFromFrontend(string message)
        {
            var payload = JsonSerializer.Deserialize<RequestPayload>(message);
            _logger.LogInformation($"{payload.Id}");
            _logger.LogInformation($"{payload.Type}");
            _logger.LogInformation($"{payload.Data}");

            switch(payload.Type)
            {
                case "ping":
                    handlePing(payload.Id, payload.Data.Deserialize<PingData>().Nonce);
                    break;
                default:
                    _logger.LogWarning($"Unknown message type: {payload.Type}");
                    break;
            }
        }

        private void handlePing(string id, ulong nonce)
        {
            _logger.LogInformation($"Handling ping with nonce {nonce}");
            SendToFrontend(JsonSerializer.Serialize(new
            {
                id,
                payload = new
                {
                    nonce
                }
            }));
        }
    }
}
