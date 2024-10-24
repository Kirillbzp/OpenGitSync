using DB.Helpers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OpenGitSync.Server.Services.Git
{

    public interface IGitHubHTTPService
    {
        Task<(bool, string)> CheckConnection(string url, string token);
        Task<(List<string>, string)> GetBranches(string url, string token);
        Task<string> SyncBranch(string urlFrom, string tokenFrom, string urlTo, string tokenTo, string branchName);
        Task<bool> AnyCommits(string url, string token);
        Task CreateInitialCommit(string url, string token);
    }
    public class GitHubHTTPService : GitWorkerBase, IGitHubHTTPService
    {
        const string baseUrl = "https://api.github.com";

        public GitHubHTTPService(IUnitOfWork unitOfWork) : base(unitOfWork)
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

        #region General Methods

        public async Task<(bool, string)> CheckConnection(string url, string token)
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
                    if(response.IsSuccessStatusCode)
                    {
                       result = true;
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return (false, content);
                    }
                }
            }
            catch (Exception ex)
            {
                return (result, ex.Message);
            }
            
            return (result, string.Empty);
        }

        public async Task<bool> AnyCommits(string url, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                var (owner, repo) = GetOwnerAndRepo(url);

                // Get the latest commit SHA of the branch
                var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/commits";
                var response = await client.GetAsync(apiUrl);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task CreateInitialCommit(string url, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                var (owner, repo) = GetOwnerAndRepo(url);

                // Create the README file
                var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/contents/README.md";
                var payload = new StringContent(JsonSerializer.Serialize(new { message = "Initial commit", content = Convert.ToBase64String(Encoding.UTF8.GetBytes("# My new repository")) }), Encoding.UTF8, "application/json");
                await client.PutAsync(apiUrl, payload);
            }
        }

        private async Task<string> GetErrorMessageFromResponse(HttpResponseMessage response, string prefix = "")
        {
            string errorMessage = string.Empty;
            var content = await response.Content.ReadAsStringAsync();
            using (JsonDocument document = JsonDocument.Parse(content))
            {
                JsonElement root = document.RootElement;
                if (root.TryGetProperty("message", out JsonElement message))
                {
                    if (!string.IsNullOrWhiteSpace(prefix))
                    {
                        errorMessage += prefix + ": ";
                    }
                    errorMessage += message.GetString();
                }
            }
            return errorMessage;
        }   

        #endregion

        #region Branches

        public async Task<(List<string>, string)> GetBranches(string url, string token)
        {
            var result = new List<string>();
            var errorMessage = string.Empty;

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
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
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
                    else
                    {
                        errorMessage = await GetErrorMessageFromResponse(response, "Error in get branches");
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Unknown error in get branches";
            }
            
            return (result, errorMessage);
        }

        public async Task<string> SyncBranch(string urlFrom, string tokenFrom, string urlTo, string tokenTo, string branchName)
        {
            var errorMessage = $"Error in sync branch {branchName}";
            var result = string.Empty;
            try
            {
                // Get the latest commit SHA of the branch from the source repository
                var commitShaFrom = string.Empty;
                (commitShaFrom, result) = await GetLatestCommitSha(urlFrom, tokenFrom, branchName);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
                if (string.IsNullOrEmpty(commitShaFrom))
                {
                    return errorMessage;
                }

                var commitShaTo = string.Empty;
                (commitShaTo, result) = await SyncCommit(urlFrom, tokenFrom, urlTo, tokenTo, commitShaFrom);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
                if (string.IsNullOrEmpty(commitShaTo))
                {
                    return errorMessage;
                }

                // Update the branch in the destination repository
                result = await UpdateBranch(urlTo, tokenTo, branchName, commitShaTo);
                return result;
            }
            catch (Exception)
            {
                // Handle exception
                return errorMessage;
            }
        }

        private async Task<string> UpdateBranch(string url, string token, string branchName, string commitSha)
        {
            var errorMessage = $"Error in sync branch {branchName}";
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Update the branch
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/git/refs/heads/{branchName}";
                    var payload = new StringContent(JsonSerializer.Serialize(new { sha = commitSha }), Encoding.UTF8, "application/json");
                    var response = await client.PatchAsync(apiUrl, payload);
                    if (!response.IsSuccessStatusCode)
                    {
                        return errorMessage;
                    }
                }
            }
            catch (Exception)
            {
                // Handle exception
                return errorMessage;
            }
            return string.Empty;
        }

        #endregion

        #region Commits

        private async Task<(string, string)> GetLatestCommitSha(string url, string token, string branchName)
        {
            string sha = string.Empty;
            string errorMessage = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Get the latest commit SHA of the branch
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/git/refs/heads/{branchName}";
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("object", out JsonElement objectElement) &&
                                objectElement.TryGetProperty("sha", out JsonElement shaElement))
                            {
                                sha = shaElement.GetString();
                            }
                        }
                    }
                    else
                    {
                        errorMessage = await GetErrorMessageFromResponse(response, "Error in get latest commit sha");
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Unknown error in get latest commit sha";
            }

            return (sha, errorMessage);
        }

        private async Task<(Commit?, string)> GetCommit(string url, string token, string commitSha, bool ignoreNotFoundError = false)
        {
            Commit commitContent = null;
            string errorMessage = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Get the commit
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/git/commits/{commitSha}";
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            commitContent = new Commit(root);
                        }
                    }
                    else
                    {
                        if (response.StatusCode == HttpStatusCode.NotFound && ignoreNotFoundError)
                        {
                            return (null, string.Empty);
                        }
                        errorMessage = await GetErrorMessageFromResponse(response, "Error in get commit");
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Unknown error in get commit";
            }
            return (commitContent, errorMessage);
        }

        private async Task<(string, string)> CreateCommit(string url, string token, Commit commit)
        {
            string commitSha = string.Empty;
            string errorMessage = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Create the commit
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/git/commits";
                    var payload = new StringContent(JsonSerializer.Serialize(commit), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(apiUrl, payload);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("sha", out JsonElement shaElement))
                            {
                                commitSha = shaElement.GetString();
                            }
                        }
                    }
                    else
                    {
                       errorMessage = await GetErrorMessageFromResponse(response, "Error in create commit");
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Unknown error in create commit";
            }
            return (commitSha, errorMessage);
        }

        private async Task<(string, string)> SyncCommit(string urlFrom, string tokenFrom, string urlTo, string tokenTo, string commitSha)
        {
            var errorMessage = string.Empty;

            // Get the tree SHA of the commit from the source repository
            var treeShaFrom = string.Empty;
            (treeShaFrom, errorMessage) = await GetTreeSha(urlFrom, tokenFrom, commitSha);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return (string.Empty, errorMessage);
            }
            if (string.IsNullOrEmpty(treeShaFrom))
            {
                return (string.Empty, "Error in Commit sync");
            }

            // Get the tree from the source repository
            List<TreeItem> treeFrom = null;
            (treeFrom, errorMessage) = await GetTree(urlFrom, tokenFrom, treeShaFrom);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return (string.Empty, errorMessage);
            }
            if (treeFrom == null || treeFrom.Count == 0)
            {
                return (string.Empty, "Error in Commit sync");
            }

            // Create blobs and trees in the destination repository
            var treeTo = new List<TreeItem>();
            foreach (var item in treeFrom)
            {
                if (item.Type == "blob")
                {
                    // Get the blob from the source repository
                    var blobContent = string.Empty;
                    (blobContent, errorMessage) = await GetBlob(urlFrom, tokenFrom, item.Sha);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        return (string.Empty, errorMessage);
                    }
                    if (string.IsNullOrEmpty(blobContent))
                    {
                        return (string.Empty, "Error in Commit sync");
                    }

                    // Create the blob in the destination repository
                    var blobShaTo = string.Empty;
                    (blobShaTo, errorMessage) = await CreateBlob(urlTo, tokenTo, blobContent, item.Sha);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        return (string.Empty, errorMessage);
                    }
                    if (string.IsNullOrEmpty(blobShaTo))
                    {
                        return (string.Empty, "Error in Commit sync");
                    }

                    treeTo.Add(new TreeItem() { Path = item.Path, Mode = "100644", Type = "blob", Sha = blobShaTo });
                }
                else if (item.Type == "tree")
                {
                    // Recursively sync the tree
                    var treeShaToRec = string.Empty;
                    (treeShaToRec, errorMessage) = await SyncTree(urlFrom, tokenFrom, urlTo, tokenTo, item.Sha);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        return (string.Empty, errorMessage);
                    }
                    if (string.IsNullOrEmpty(treeShaToRec))
                    {
                        return (string.Empty, "Error in Commit sync");
                    }

                    treeTo.Add(new TreeItem() { Path = item.Path, Mode = "040000", Type = "tree", Sha = treeShaToRec });
                }
            }

            // Create the tree in the destination repository
            var treeShaTo = string.Empty;
            (treeShaTo, errorMessage) = await CreateTree(urlTo, tokenTo, treeTo);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return (string.Empty, errorMessage);
            }

            if (string.IsNullOrEmpty(treeShaTo))
            {
                return (string.Empty, "Error in Commit sync");
            }

            Commit commit = null;
            (commit, errorMessage) = await GetCommit(urlFrom, tokenFrom, commitSha);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return (string.Empty, errorMessage);
            }
            if (commit == null)
            {
                return (string.Empty, "Error in Commit sync");
            }

            commit.Tree = treeShaTo;

            if (commit.Parents.Count > 0)
            {
                foreach (var parentSha in commit.Parents)
                {
                    Commit parentCommit = null;
                    (parentCommit, errorMessage) = await GetCommit(urlTo, tokenTo, parentSha, true);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        return (string.Empty, errorMessage);
                    }

                    if (parentCommit == null)
                    {
                        var newParentSha = string.Empty;
                        (newParentSha, errorMessage) = await SyncCommit(urlFrom, tokenFrom, urlTo, tokenTo, parentSha);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return (string.Empty, errorMessage);
                        }
                    }
                }
            }

            // Create the commit in the destination repository
            var commitShaTo = string.Empty;
            (commitShaTo, errorMessage) = await CreateCommit(urlTo, tokenTo, commit);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return (string.Empty, errorMessage);
            }
            if (string.IsNullOrEmpty(commitShaTo))
            {
                return (string.Empty, "Error in Commit sync");
            }

            return (commitShaTo, string.Empty);
        }

        private async Task<(bool,string)> SyncBranchCommits(string urlFrom, string tokenFrom, string urlTo, string tokenTo, string branchName)
        {
            // Get the list of commits from the specific branch in the source repository
            var (commits, errorMessage) = await GetBranchCommits(urlFrom, tokenFrom, branchName);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // Handle error
                return (false, errorMessage);
            }

            // Create each commit in the target repository
            foreach (var commit in commits)
            {
                // Create the commit in the target repository
                await CreateCommit(urlTo, tokenTo, commit);
            }
            return (true, string.Empty);
        }

        private async Task<(List<Commit>, string)> GetBranchCommits(string url, string token, string branchName)
        {
            var commits = new List<Commit>();
            var errorMessage = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Get the list of commits from the specific branch
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/commits?sha={branchName}";
                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            foreach (JsonElement commitElement in root.EnumerateArray())
                            {
                                var commit = new Commit(commitElement);
                                commits.Add(commit);
                            }
                        }
                    }
                    else
                    {
                        errorMessage = await GetErrorMessageFromResponse(response, "Error in get branch commits");
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Unknown error in get branch commits";
            }
            return (commits, errorMessage);
        }

        #endregion

        #region Trees

        private async Task<(string, string)> SyncTree(string urlFrom, string tokenFrom, string urlTo, string tokenTo, string treeShaFrom)
        {
            var treeShaTo = string.Empty;
            var errorMessage = string.Empty;

            try
            {
                // Get the tree from the source repository
                List<TreeItem> treeFrom = null;
                (treeFrom, errorMessage) = await GetTree(urlFrom, tokenFrom, treeShaFrom);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return (string.Empty, errorMessage);
                }
                if (treeFrom == null || treeFrom.Count == 0)
                {
                    return (string.Empty, "Error in Sync Tree");
                }

                // Create blobs and trees in the destination repository
                var treeTo = new List<TreeItem>();
                foreach (var item in treeFrom)
                {
                    if (item.Type == "blob")
                    {
                        // Get the blob from the source repository
                        var blobContent = string.Empty;
                        (blobContent, errorMessage) = await GetBlob(urlFrom, tokenFrom, item.Sha);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return (string.Empty, errorMessage);
                        }
                        if (string.IsNullOrEmpty(blobContent))
                        {
                            return (string.Empty, "Error in Sync Tree");
                        }

                        // Create the blob in the destination repository
                        var blobShaTo = string.Empty;
                        (blobShaTo, errorMessage) = await CreateBlob(urlTo, tokenTo, blobContent, item.Sha);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return (string.Empty, errorMessage);
                        }
                        if (string.IsNullOrEmpty(blobShaTo))
                        {
                            return (string.Empty, "Error in Sync Tree");
                        }

                        treeTo.Add(new TreeItem() { Path = item.Path, Mode = "100644", Type = "blob", Sha = blobShaTo });
                    }
                    else if (item.Type == "tree")
                    {
                        // Recursively sync the tree
                        var treeShaToRec = string.Empty;
                        (treeShaToRec, errorMessage) = await SyncTree(urlFrom, tokenFrom, urlTo, tokenTo, item.Sha);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return (string.Empty, errorMessage);
                        }
                        if (string.IsNullOrEmpty(treeShaToRec))
                        {
                            return (string.Empty, "Error in Sync Tree");
                        }

                        treeTo.Add(new TreeItem(){Path = item.Path, Mode = "040000", Type = "tree", Sha = treeShaToRec});
                    }
                }

                // Create the tree in the destination repository
                (treeShaTo, errorMessage) = await CreateTree(urlTo, tokenTo, treeTo);
            }
            catch (Exception)
            {
                // Handle exception
                errorMessage = "Error in Sync Tree";
            }
            return (treeShaTo, errorMessage);
        }

        private async Task<(string, string)> GetTreeSha(string url, string token, string commitSha)
        {
            string treeSha = string.Empty;
            string errorMessage = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Get the tree SHA of the commit
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/git/commits/{commitSha}";
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("tree", out JsonElement treeElement) &&
                                treeElement.TryGetProperty("sha", out JsonElement shaElement))
                            {
                                treeSha = shaElement.GetString();
                            }
                        }
                    }
                    else
                    {
                        errorMessage = await GetErrorMessageFromResponse(response, "Error in get tree sha");
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Unknown error in get tree sha";
            }
            return (treeSha, errorMessage);
        }

        private async Task<(List<TreeItem>, string)> GetTree(string url, string token, string treeSha)
        {
            var tree = new List<TreeItem>();
            var errormessage = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Get the tree
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/git/trees/{treeSha}";
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("tree", out JsonElement treeElement))
                            {
                                foreach (JsonElement item in treeElement.EnumerateArray())
                                {
                                    if (item.TryGetProperty("type", out JsonElement typeElement) &&
                                        item.TryGetProperty("path", out JsonElement pathElement) &&
                                        item.TryGetProperty("sha", out JsonElement shaElement))
                                    {
                                        tree.Add(new TreeItem() { Type = typeElement.GetString(), 
                                                                  Path = pathElement.GetString(), 
                                                                  Sha = shaElement.GetString() });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        errormessage = await GetErrorMessageFromResponse(response, "Error in get tree");
                    }
                }
            }
            catch (Exception)
            {
                errormessage = "Unknown error in get tree";
            }
            return (tree, errormessage);
        }

        private async Task<(string, string)> CreateTree(string url, string token, List<TreeItem> tree)
        {
            string treeSha = string.Empty;
            string errorMessage = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Create the tree
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/git/trees";
                    //var payload = new StringContent(JsonSerializer.Serialize(new { tree = tree.Select(t => new { path = t.Path, mode = t.Mode, type = t.Type, sha = t.Sha }) }), Encoding.UTF8, "application/json");
                    var jsontree1 = JsonSerializer.Serialize(new { tree = tree.Select(t => new { path = t.Path, mode = t.Mode, type = t.Type, sha = t.Sha }) });
                    var jsontree2 = JsonSerializer.Serialize(new { tree = tree });
                    var payload = new StringContent(JsonSerializer.Serialize(new { tree = tree }), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(apiUrl, payload);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("sha", out JsonElement shaElement))
                            {
                                treeSha = shaElement.GetString();
                            }
                        }
                    }
                    else
                    {
                        errorMessage = await GetErrorMessageFromResponse(response, "Error in create tree");
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Unknown error in create tree";
            }
            return (treeSha, errorMessage);
        }

        #endregion

        #region Blobs

        private async Task<(string, string)> GetBlob(string url, string token, string blobSha)
        {
            string blobContent = string.Empty;
            string errorMessage = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Get the blob
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/git/blobs/{blobSha}";
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("content", out JsonElement contentElement))
                            {
                                blobContent = contentElement.GetString();
                            }
                        }
                    }
                    else
                    {
                       errorMessage = await GetErrorMessageFromResponse(response, "Error in get blob");
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Unknown error in get blob";
            }
            return (blobContent, errorMessage);
        }

        private async Task<(string, string)> CreateBlob(string url, string token, string content, string sha)
        {
            string blobSha = string.Empty;
            string errorMessage = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/git/blobs";

                    var (existingBlobContent, resultErrorMessage) = await GetBlob(url, token, sha);
                    if (!string.IsNullOrEmpty(resultErrorMessage))
                    {
                        return (string.Empty, resultErrorMessage);
                    }
                    if (existingBlobContent != null)
                    {
                        // The blob already exists, return its SHA-1 hash
                        return (sha, errorMessage);
                    }

                    var payload = new StringContent(JsonSerializer.Serialize(new { content = content, encoding = "base64" }), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(apiUrl, payload);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(responseContent))
                        {
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("sha", out JsonElement shaElement))
                            {
                                blobSha = shaElement.GetString();
                            }
                        }
                    }
                    else
                    {
                        errorMessage = await GetErrorMessageFromResponse(response, "Error in create blob");
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Unknown error in create blob";
            }
            return (blobSha, errorMessage);
        }

        #endregion
        
    }
}
