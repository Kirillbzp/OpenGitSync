using DB.Models;

namespace DB.Helpers
{
    public interface ISyncSettingRepository
    {
        SyncSetting GetSyncSettingById(long id);
        IEnumerable<SyncSetting> GetSyncSettingsByProjectId(long projectId);
        void AddSyncSetting(SyncSetting syncSetting);
        void UpdateSyncSetting(SyncSetting syncSetting);
        void DeleteSyncSetting(SyncSetting syncSetting);
    }

    public class SyncSettingRepository : ISyncSettingRepository
    {
        private readonly AppDbContext _dbContext;

        public SyncSettingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SyncSetting GetSyncSettingById(long id)
        {
            return _dbContext.SyncSettings.Find(id);
        }

        public IEnumerable<SyncSetting> GetSyncSettingsByProjectId(long projectId)
        {
            return _dbContext.SyncSettings.Where(s => s.ProjectId == projectId).ToList();
        }

        public void AddSyncSetting(SyncSetting syncSetting)
        {
            _dbContext.SyncSettings.Add(syncSetting);
        }

        public void UpdateSyncSetting(SyncSetting syncSetting)
        {
            _dbContext.SyncSettings.Update(syncSetting);
        }

        public void DeleteSyncSetting(SyncSetting syncSetting)
        {
            _dbContext.SyncSettings.Remove(syncSetting);
        }
    }

}
