using Newtonsoft.Json;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Client.Services
{
    public interface ISyncSettingsService
    {
        Task<List<SyncSettingDto>> GetSyncSettings();
    }

    public class SyncSettingsService : ISyncSettingsService
    {
        private readonly HttpClient _httpClient;

        public SyncSettingsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SyncSettingDto>> GetSyncSettings()
        {
            var response = await _httpClient.GetAsync("api/syncsettings");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var syncSettings = JsonConvert.DeserializeObject<List<SyncSettingDto>>(content);
                return syncSettings;
            }

            // Handle unsuccessful response
            throw new Exception("Failed to retrieve sync settings.");
        }
    }
}
