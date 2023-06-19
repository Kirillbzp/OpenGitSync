using DB.Helpers;
using DB.Models;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface ISyncSettingService
    {
        SyncSetting GetSyncSettingById(long id);
        IEnumerable<SyncSetting> GetSyncSettingsByProjectId(long projectId);
        void CreateSyncSetting(SyncSetting syncSetting);
        void UpdateSyncSetting(SyncSetting syncSetting);
        void DeleteSyncSetting(SyncSetting syncSetting);
    }

    public class SyncSettingService : ISyncSettingService
    {
        private readonly ISyncSettingRepository _syncSettingRepository;

        public SyncSettingService(ISyncSettingRepository syncSettingRepository)
        {
            _syncSettingRepository = syncSettingRepository;
        }

        public SyncSetting GetSyncSettingById(long id)
        {
            return _syncSettingRepository.GetSyncSettingById(id);
        }

        public IEnumerable<SyncSetting> GetSyncSettingsByProjectId(long projectId)
        {
            return _syncSettingRepository.GetSyncSettingsByProjectId(projectId);
        }

        public void CreateSyncSetting(SyncSetting syncSetting)
        {
            _syncSettingRepository.AddSyncSetting(syncSetting);
        }

        public void UpdateSyncSetting(SyncSetting syncSetting)
        {
            _syncSettingRepository.UpdateSyncSetting(syncSetting);
        }

        public void DeleteSyncSetting(SyncSetting syncSetting)
        {
            _syncSettingRepository.DeleteSyncSetting(syncSetting);
        }

    }

}
