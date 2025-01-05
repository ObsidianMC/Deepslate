using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Deepslate.LauncherLib.Entities
{
    public struct VersionMetadata
    {
        [JsonPropertyName("assetIndex")]
        public AssetIndex AssetIndex { get; set; }

        [JsonPropertyName("assets")]
        public string Assets { get; set; }

        [JsonPropertyName("downloads")]
        public Downloadables Downloads { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("libraries")]
        public List<Library> Libraries { get; set; }

        [JsonPropertyName("mainClass")]
        public string MainClass { get; set; }

        [JsonPropertyName("releaseTime")]
        public DateTimeOffset ReleaseTime { get; set; }

        [JsonPropertyName("time")]
        public DateTimeOffset Time { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("javaVersion")]
        public JavaVersion JavaVersion { get; set; }
    }

    public struct JavaVersion
    {
        [JsonPropertyName("component")]
        public string Component { get; set; }

        [JsonPropertyName("majorVersion")]
        public int MajorVersion { get; set; }
    }

    public struct Library
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public struct AssetIndex
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("sha1")]
        public string Sha1 { get; set; }

        [JsonPropertyName("totalSize")]
        public long TotalSize { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public struct Downloadables
    {
        [JsonPropertyName("client")]
        public Downloadable Client { get; set; }

        [JsonPropertyName("client_mappings")]
        public Downloadable ClientMappings { get; set; }

        [JsonPropertyName("server")]
        public Downloadable Server { get; set; }

        [JsonPropertyName("server_mappings")]
        public Downloadable ServerMappings { get; set; }
    }

    public struct Downloadable
    {
        [JsonPropertyName("sha1")]
        public string Sha1 { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
