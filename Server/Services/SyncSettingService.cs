using DB.Helpers;
using DB.Models;

namespace OpenGitSync.Server.Services
{
    public interface ISyncSettingService
    {
        Task<SyncSetting> GetSyncSettingById(long id);
        Task<IEnumerable<SyncSetting>> GetSyncSettingsByProjectId(long projectId);
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

        public async Task<SyncSetting> GetSyncSettingById(long id)
        {
            return await _syncSettingRepository.GetSyncSettingById(id);
        }

        public async Task<IEnumerable<SyncSetting>> GetSyncSettingsByProjectId(long projectId)
        {
            return await _syncSettingRepository.GetSyncSettingsByProjectId(projectId);
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
