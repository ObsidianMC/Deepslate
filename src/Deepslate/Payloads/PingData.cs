using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Deepslate.Payloads
{
    public class PingData
    {
        [JsonPropertyName("nonce")]
        public ulong Nonce { get; set; }
    }
}
