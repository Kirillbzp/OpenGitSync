using DB.Helpers;
using LibGit2Sharp;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OpenGitSync.Server.Services.Git
{

    public interface IGitHubService
    {
        Task<(bool, string)> CheckConnection(string url, string token);
        Task<(List<string>, string)> GetBranches(string url, string token);
        Task<string> SyncAll(string urlFrom, string tokenFrom, string urlTo, string tokenTo);
        Task<string> SyncBranch(string urlFrom, string tokenFrom, string urlTo, string tokenTo, string branchName);
        Task<bool> AnyCommits(string url, string token);
        Task CreateInitialCommit(string url, string token);
    }
    public class GitHubService : GitWorkerBase, IGitHubService
    {
        const string baseUrl = "https://api.github.com";

        public GitHubService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

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
                    if (response.IsSuccessStatusCode)
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

        private async Task CreateInitialWikiPage(string wikiUrl, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                var (owner, repo) = GetOwnerAndRepo(wikiUrl);

                // Create the initial page in the wiki
                var apiUrl = $"{baseUrl}/repos/{owner}/{repo}/contents/Home.md";
                var payload = new StringContent(JsonSerializer.Serialize(new { message = "Initial commit", content = Convert.ToBase64String(Encoding.UTF8.GetBytes("# Home")) }), Encoding.UTF8, "application/json");
                await client.PutAsync(apiUrl, payload);
            }
        }
        private async Task SetDefaultBranch(string url, string token, string defaultBranch)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                var (owner, repo) = GetOwnerAndRepo(url);

                // Construct the API endpoint URL
                var apiUrl = $"{baseUrl}/repos/{owner}/{repo}";

                var content = new StringContent(JsonSerializer.Serialize(new { default_branch = defaultBranch }), Encoding.UTF8, "application/json");

                var response = await client.PatchAsync(apiUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await GetErrorMessageFromResponse(response, "Error setting default branch");
                    throw new Exception(errorMessage);
                }

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

        public async Task<bool> IsWikiEnabled(string url, string token)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                    var (owner, repo) = GetOwnerAndRepo(url);

                    // Construct the API endpoint URL to get repository details
                    var apiUrl = $"{baseUrl}/repos/{owner}/{repo}";

                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using (JsonDocument document = JsonDocument.Parse(content))
                        {
                            JsonElement root = document.RootElement;
                            if (root.TryGetProperty("has_wiki", out JsonElement hasWiki))
                            {
                                return hasWiki.GetBoolean();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them as needed
                Console.WriteLine($"Error checking Wiki status: {ex.Message}");
            }

            return false;
        }

        // Get the WiKi URL from the repository URL:
        // baseAddress/owner/repository.git -> baseAddress/owner/repository.wiki.git
        private string GetWiKiUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            var (owner, repo) = GetOwnerAndRepo(url);

            // Construct the API endpoint URL
            return $"https://github.com/{owner}/{repo}.wiki.git";

            //if (url.EndsWith("/wiki"))
            //{
            //    return url + ".git";
            //}

            //if (url.EndsWith(".git"))
            //{
            //    return url.Substring(0, url.Length - 4) + "/wiki.git";
            //}

            //return url + "/wiki.git";
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
            string tempDir = string.Empty;
            try
            {
                // Temporary directory for cloning
                tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDir);

                // Clone options for source repository
                var co = new CloneOptions();
                co.FetchOptions.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = tokenFrom, Password = string.Empty };
                co.BranchName = branchName;
                co.IsBare = false;

                // Clone the source repository
                Repository.Clone(urlFrom, tempDir, co);
                //Repository.Init(tempDir, true);
                using (var repo = new Repository(tempDir))
                {

                    // Add remote for destination repository
                    var remoteNameTo = "copy";
                    repo.Network.Remotes.Add(remoteNameTo, urlTo);
                    var remote = repo.Network.Remotes[remoteNameTo];

                    // Push options for destination repository
                    var po = new PushOptions();
                    po.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = tokenTo, Password = string.Empty };

                    // Push the branch to the destination repository
                    repo.Network.Push(remote, $"refs/heads/{branchName}", po);

                }

            }
            catch (Exception ex)
            {
                return $"Error syncing branch: {ex.Message}";
            }
            finally
            {
                await RemoveDirectory(tempDir);
            }

            return "";
        }

        public async Task<string> SyncAll(string urlFrom, string tokenFrom, string urlTo, string tokenTo)
        {
            string tempDir = string.Empty;
            string errorMessage = string.Empty;
            try
            {
                // Temporary directory for cloning
                tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDir);

                // Clone options for source repository
                var co = new CloneOptions();
                co.FetchOptions.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = tokenFrom, Password = string.Empty };
                co.IsBare = false;

                // Clone the source repository
                Repository.Clone(urlFrom, tempDir, co);
                using (var repo = new Repository(tempDir))
                {
                    // Add remote for destination repository
                    var remoteNameTo = "copy";
                    repo.Network.Remotes.Add(remoteNameTo, urlTo);
                    var remote = repo.Network.Remotes[remoteNameTo];

                    // Fetch all branches from the remote
                    Commands.Fetch(repo, "origin", new string[0], new FetchOptions
                    {
                        CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = tokenFrom, Password = string.Empty }
                    }, null);

                    // Push options for destination repository
                    var po = new PushOptions();
                    po.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = tokenTo, Password = string.Empty };
                    po.OnPushStatusError = PushStatusErrorHandler;

                    //foreach (var branch in repo.Branches.Where(b => b.IsRemote && b.FriendlyName == "origin/HEAD"))
                    //{
                    //    var branchName = branch.FriendlyName.Substring("origin/".Length);
                    //    var localBranch = repo.Branches[branchName] ?? repo.CreateBranch(branchName, branch.Tip);
                    //    Commands.Checkout(repo, localBranch);
                    //    repo.Network.Push(remote, localBranch.CanonicalName, po);
                    //}

                    // Determine the default branch in the origin repository
                    var defaultBranch = repo.Head.FriendlyName;

                    foreach (var branch in repo.Branches.Where(b => b.FriendlyName == "origin/" + defaultBranch && b.IsRemote))
                    {
                        var localBranch = repo.Branches[defaultBranch] ?? repo.CreateBranch(defaultBranch, branch.Tip);
                        Commands.Checkout(repo, localBranch);
                        repo.Network.Push(remote, $"+refs/heads/{defaultBranch}", po);
                    }

                    await SetDefaultBranch(urlTo, tokenTo, defaultBranch);

                    foreach (var branch in repo.Branches.Where(b => b.FriendlyName != "origin/" + defaultBranch && b.IsRemote && b.FriendlyName != "origin/HEAD"))
                    {
                        var branchName = branch.FriendlyName.Substring("origin/".Length);
                        var localBranch = repo.Branches[branchName] ?? repo.CreateBranch(branchName, branch.Tip);
                        Commands.Checkout(repo, localBranch);
                        repo.Network.Push(remote, localBranch.CanonicalName, po);
                    }

                    foreach (var tag in repo.Tags)
                    {
                        // Push the tag to the destination repository
                        repo.Network.Push(remote, $"refs/tags/{tag.FriendlyName}", po);
                    }

                    errorMessage = await SyncWiki(urlFrom, tokenFrom, urlTo, tokenTo);
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error syncing branch: {ex.Message}";
            }
            finally
            {
                await RemoveDirectory(tempDir);
            }

            return errorMessage;
        }

        public void PushStatusErrorHandler(PushStatusError pushStatusErrors)
        {
            var errorMessage = pushStatusErrors.Message;
            throw new Exception(errorMessage);
        }

        private async Task<string> SyncWiki (string urlFrom, string tokenFrom, string urlTo, string tokenTo)
        {
            string tempDir = string.Empty;
            
            if (!await IsWikiEnabled(urlFrom, tokenFrom) ||
                !await IsWikiEnabled(urlTo, tokenTo) ||
                !await AnyCommits(GetWiKiUrl(urlFrom), tokenFrom))
            {
                //return "WiKi is not enabled in the source or destination repository";
                return "";
            }

            try
            {
                var wikiUrlFrom = GetWiKiUrl(urlFrom);
                var wikiUrlTo = GetWiKiUrl(urlTo);
                // Temporary directory for cloning
                tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDir);

                // Clone options for source repository
                var co = new CloneOptions();
                co.FetchOptions.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = tokenFrom, Password = string.Empty };
                co.IsBare = false;

                //Clone the wiki repository
                Repository.Clone(wikiUrlFrom, tempDir, co);
                using (var repo = new Repository(tempDir))
                {
                    // Add remote for destination repository
                    var remoteNameTo = "copy";
                    repo.Network.Remotes.Add(remoteNameTo, wikiUrlTo);
                    var remote = repo.Network.Remotes[remoteNameTo];

                    // Push options for destination repository
                    var po = new PushOptions();
                    po.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = tokenTo, Password = string.Empty };

                    var defaultBranch = repo.Head.FriendlyName;

                    var localBranch = repo.Branches[defaultBranch];
                    Commands.Checkout(repo, localBranch);



                    // Check if the destination wiki is empty
                    if (!await AnyCommits(wikiUrlTo, tokenTo))
                    {
                        // Create an initial commit in the destination wiki
                        await CreateInitialWikiPage(wikiUrlTo, tokenTo);
                    }

                    repo.Network.Push(remote, localBranch.CanonicalName, po);

                    // Push the branch to the destination repository
                    //repo.Network.Push(remote, $"refs/heads/{defaultBranch}", po);
                }
            }
            catch (Exception ex)
            {
                return $"Error syncing WiKi: {ex.Message}";
            }
            finally
            {
                await RemoveDirectory(tempDir);
            }

            return "";
           
        }

        #endregion

        #region Commits



        #endregion

        #region Trees



        #endregion

        #region Blobs



        #endregion

    }
}
