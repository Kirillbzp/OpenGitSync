using OpenGitSync.Shared.DataTransferObjects;
using System.Net.Http.Json;

namespace OpenGitSync.Client.Services
{
    interface ISyncWorkerService
    {
        Task<string> SyncAll(string syncSettingsId);
    }
    public class SyncWorkerService : ISyncWorkerService
    {
        private readonly HttpClient _httpClient;

        public SyncWorkerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SyncAll(string syncSettingsId)
        {
            var result = await _httpClient.GetAsync($"api/syncworker/syncAll/{syncSettingsId}");
            if (result.IsSuccessStatusCode)
            {
                return string.Empty;
            }
            return result.Content.ReadAsStringAsync().Result;
        }
    }
}
