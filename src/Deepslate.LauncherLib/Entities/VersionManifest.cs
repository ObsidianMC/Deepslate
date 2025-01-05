using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Deepslate.LauncherLib.Entities
{
    public struct VersionManifest
    {
        [JsonPropertyName("latest")]
        public LatestVersion Latest { get; set; }

        [JsonPropertyName("versions")]
        public List<Version> Versions { get; set; }
    }

    public struct LatestVersion
    {
        [JsonPropertyName("release")]
        public string Release { get; set; }

        [JsonPropertyName("snapshot")]
        public string Snapshot { get; set; }
    }

    public struct Version
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("url")]
        public string GameVersionManifestUrl { get; set; }

        [JsonPropertyName("time")]
        public DateTimeOffset Time { get; set; }

        [JsonPropertyName("releaseTime")]
        public DateTimeOffset ReleaseTime { get; set; }

        [JsonPropertyName("sha1")]
        public string Sha1 { get; set; }

        [JsonPropertyName("complianceLevel")]
        public int ComplianceLevel { get; set; }
    }
}
