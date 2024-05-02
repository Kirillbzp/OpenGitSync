using Newtonsoft.Json;
using OpenGitSync.Shared.DataTransferObjects;
using System.Net.Http.Json;

namespace OpenGitSync.Client.Services
{
    public interface ISyncSettingsService
    {
        Task<List<SyncSettingDto>> GetSyncSettings();
        Task<SyncSettingDto> GetSyncSettingById(string id);
        Task<List<SyncSettingDto>> GetProjectSyncSettings(string projectId);
        Task<(bool, SyncSettingDto)> CreateSyncSetting(SyncSettingCreateDto syncSettingCreateDto);
        Task<bool> UpdateSyncSetting(long id, SyncSettingDto syncSettingUpdateDto);
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
            var response = await _httpClient.GetFromJsonAsyncSafe<List<SyncSettingDto>>("api/syncsettings");

            if (response == null)
            {
                return new List<SyncSettingDto>();
            }

            return response;
        }

        public async Task<SyncSettingDto> GetSyncSettingById(string id)
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<SyncSettingDto>($"api/syncsettings/{id}");
            if (result == null)
            {
                return new SyncSettingDto();
            }
            return result;
        }

        public async Task<List<SyncSettingDto>> GetProjectSyncSettings(string projectId)
        {
            var result = await _httpClient.GetFromJsonAsyncSafe<List<SyncSettingDto>>($"api/syncsettings/project/{projectId}");
            if (result == null)
            {
                return new List<SyncSettingDto>();
            }
            return result;
        }

        public async Task<(bool, SyncSettingDto)> CreateSyncSetting(SyncSettingCreateDto syncSettingCreateDto)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/syncsettings", syncSettingCreateDto);
                var syncSetting = await result.Content.ReadFromJsonAsync<SyncSettingDto>();
                if (syncSetting == null)
                {
                    return (false, null);
                }
                return (result.IsSuccessStatusCode, syncSetting);
            }
            catch
            {
                return (false, null);
            }
        }

        public async Task<bool> UpdateSyncSetting(long id, SyncSettingDto syncSettingUpdateDto)
        {
            try
            {
                var result = await _httpClient.PutAsJsonAsync($"api/syncsettings/{id}", syncSettingUpdateDto);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }


    }
}
