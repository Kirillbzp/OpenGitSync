using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenGitSync.Server.Services.Git
{
    public class Commit
    {
        [JsonPropertyName("sha")]
        public string Sha { get; set; }
        [JsonPropertyName("parents")]
        public List<string> Parents { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("tree")]
        public string Tree { get; set; }
        [JsonPropertyName("author")]
        public Author Author { get; set; }
        [JsonPropertyName("committer")]
        public Author Committer { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        public Commit()
        {
            Sha = string.Empty;
            Message = string.Empty;
            Tree = string.Empty;
            Author = new Author();
            Committer = new Author();
            Signature = string.Empty;
            Parents = new List<string>();
        }

        public Commit(JsonElement commitElement)
        {
            if (commitElement.TryGetProperty("sha", out JsonElement shaElement))
            {
                this.Sha = shaElement.GetString();
            }
            else
            {
                this.Sha = string.Empty;
            }

            if (commitElement.TryGetProperty("message", out JsonElement messageElement))
            {
                this.Message = messageElement.GetString();
            }
            else
            {
                this.Message = string.Empty;
            }

            if (commitElement.TryGetProperty("tree", out JsonElement treeElement) &&
                treeElement.TryGetProperty("sha", out JsonElement treeShaElement))
            {
                this.Tree = treeShaElement.GetString();
            }
            else
            {
                this.Tree = string.Empty;
            }

            if(commitElement.TryGetProperty("signature", out JsonElement signatureElement))
            {
                this.Signature = signatureElement.GetString();
            }
            else
            {
                this.Signature = string.Empty;
            }

            this.Parents = new List<string>();
            if (commitElement.TryGetProperty("parents", out JsonElement parentsElement))
            {
                foreach (JsonElement parentElement in parentsElement.EnumerateArray())
                {
                    if (parentElement.TryGetProperty("sha", out JsonElement parentShaElement))
                    {
                        this.Parents.Add(parentShaElement.GetString());
                    }
                }
            }

            this.Author = new Author();
            if (commitElement.TryGetProperty("author", out JsonElement authorElement))
            {
                if (authorElement.TryGetProperty("name", out JsonElement nameElement))
                {
                    this.Author.Name = nameElement.GetString();
                }
                if (authorElement.TryGetProperty("email", out JsonElement emailElement))
                {
                    this.Author.Email = emailElement.GetString();
                }
                if (authorElement.TryGetProperty("date", out JsonElement dateElement))
                {
                    this.Author.Date = dateElement.GetString();
                }
            }

            this.Committer = new Author();
            if (commitElement.TryGetProperty("committer", out JsonElement committerElement))
            {
                if (committerElement.TryGetProperty("name", out JsonElement nameElement))
                {
                    this.Committer.Name = nameElement.GetString();
                }
                if (committerElement.TryGetProperty("email", out JsonElement emailElement))
                {
                    this.Committer.Email = emailElement.GetString();
                }
                if (committerElement.TryGetProperty("date", out JsonElement dateElement))
                {
                    this.Committer.Date = dateElement.GetString();
                }
            }
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
