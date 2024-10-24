using System.Text.Json.Serialization;

namespace OpenGitSync.Server.Services.Git
{
    public class TreeItem
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("path")]
        public string Path { get; set; }
        [JsonPropertyName("sha")]
        public string Sha { get; set; }
        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        public TreeItem()
        {
            Type = string.Empty;
            Path = string.Empty;
            Sha = string.Empty;
            Mode = string.Empty;
        }
    }
}
