using DB.Models;
using LibGit2Sharp;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OpenGitSync.Server.Helpers.Git
{
    public interface IGitHubService
    {
        public Task<bool> CheckConnection(string url, string token);
    }
    public class GitHubService : GitWorkerBase, IGitHubService
    {
        //private string repoAddress = "https://github.com/Kirillbzp/OpenGitSync.git";
        //private string token = "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";

        const string baseUrl = "https://api.github.com";

        public GitHubService() : base()
        {
            
        }

        private (string, string) GetOwnerAndRepo(string url)
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            var owner = segments[1];
            var repo = segments[2];

            if (repo.EndsWith(".git"))
            {
                repo = repo.Substring(0, repo.Length - 4);
            }

            return (owner, repo);
        }

        public async Task<bool> CheckConnection(string url, string token)
        {
            
            var result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Construct the API endpoint URL
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}";

                    var response = await client.GetAsync(apiUrl);

                    // If the status code is 200, the connection is successful
                    result = response.IsSuccessStatusCode;
                }
            }
            catch (Exception)
            {
                // result = false;
            }
            finally
            {
                
            }
            return result;
        }

        public async Task<List<string>> GetBranches(string url, string token)
        {

            var result = new List<string>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Construct the API endpoint URL
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/branches";

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            foreach (JsonElement branch in root.EnumerateArray())
                            {
                                if (branch.TryGetProperty("name", out JsonElement nameElement))
                                {
                                    result.Add(nameElement.GetString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // result = false;
            }
            finally
            {

            }
            return result;
        }

        public async Task<bool> SyncBranch(string urlFrom, string tokenFrom, string urlTo, string tokenTo, string branchName)
        {

            var result = false;
            try
            {
                using (var clientFrom = new HttpClient())
                using (var clientTo = new HttpClient())
                {
                    clientFrom.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    clientFrom.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", tokenFrom);

                    clientTo.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    clientTo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", tokenTo);

                    var (ownerFrom, repoFrom) = GetOwnerAndRepo(urlFrom);
                    var (ownerTo, repoTo) = GetOwnerAndRepo(urlTo);

                    var apiUrl = $"{baseUrl}/repos/{ownerFrom}/{repoFrom}/git/refs/heads/{branchName}";
                    var response = await clientFrom.GetAsync(apiUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }
                    var content = await response.Content.ReadAsStringAsync();
                    using (JsonDocument document = JsonDocument.Parse(content))
                    {
                        JsonElement root = document.RootElement;
                        if (!root.TryGetProperty("object", out JsonElement objectElement) ||
                            !objectElement.TryGetProperty("sha", out JsonElement shaElement))
                        {
                            return false;
                        }
                        var sha = shaElement.GetString();

                        // Create or update the branch in the destination repository
                        apiUrl = $"{baseUrl}/repos/{ownerTo}/{repoTo}/git/refs/heads/{branchName}";
                        var payload = new StringContent(JsonSerializer.Serialize(new { @ref = $"refs/heads/{branchName}", sha = sha }), Encoding.UTF8, "application/json");
                        response = await clientTo.PostAsync(apiUrl, payload);
                        if (!response.IsSuccessStatusCode)
                        {
                            // If the branch already exists, update it
                            if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                            {
                                response = await clientTo.PatchAsync(apiUrl, payload);
                            }
                        }
                        result = response.IsSuccessStatusCode;
                    }

                }
            }
            catch (Exception)
            {
                // result = false;
            }
            finally
            {

            }
            return result;
        }
    }
}
