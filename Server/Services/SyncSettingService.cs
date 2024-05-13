using DB.Helpers;
using DB.Models;

namespace OpenGitSync.Server.Services
{
    public interface ISyncSettingService
    {
        Task<SyncSetting?> GetSyncSettingById(long id, string userId);
        Task<IEnumerable<SyncSetting>> GetSyncSettingsByProjectId(long projectId, string userId);
        Task CreateSyncSetting(SyncSetting syncSetting);
        Task UpdateSyncSetting(SyncSetting syncSetting);
        Task DeleteSyncSetting(SyncSetting syncSetting);
    }

    public class SyncSettingService : ISyncSettingService
    {
        private readonly ISyncSettingRepository _syncSettingRepository;

        public SyncSettingService(ISyncSettingRepository syncSettingRepository)
        {
            _syncSettingRepository = syncSettingRepository;
        }

        public async Task<SyncSetting?> GetSyncSettingById(long id, string userId)
        {
            return await _syncSettingRepository.GetSyncSettingById(id, userId);
        }

        public async Task<IEnumerable<SyncSetting>> GetSyncSettingsByProjectId(long projectId, string userId)
        {
            return await _syncSettingRepository.GetSyncSettingsByProjectId(projectId, userId);
        }

        public async Task CreateSyncSetting(SyncSetting syncSetting)
        {
            await _syncSettingRepository.AddSyncSetting(syncSetting);
        }

        public async Task UpdateSyncSetting(SyncSetting syncSetting)
        {
            await _syncSettingRepository.UpdateSyncSetting(syncSetting);
        }

        public async Task DeleteSyncSetting(SyncSetting syncSetting)
        {
            await _syncSettingRepository.DeleteSyncSetting(syncSetting);
        }

    }

}
