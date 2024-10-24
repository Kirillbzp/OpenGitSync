using System.Text.Json.Serialization;

namespace OpenGitSync.Server.Services.Git
{
    public class Author
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        public Author() 
        { 
            Name = string.Empty; 
            Email = string.Empty; 
            Date = string.Empty;
        }
    }
}
