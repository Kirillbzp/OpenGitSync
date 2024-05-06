using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface ISyncSettingRepository
    {
        Task<SyncSetting> GetSyncSettingById(long id);
        Task<IEnumerable<SyncSetting>> GetSyncSettingsByProjectId(long projectId);
        Task AddSyncSetting(SyncSetting syncSetting);
        Task UpdateSyncSetting(SyncSetting syncSetting);
        Task DeleteSyncSetting(SyncSetting syncSetting);
    }

    public class SyncSettingRepository : ISyncSettingRepository
    {
        private readonly AppDbContext _dbContext;

        public SyncSettingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SyncSetting> GetSyncSettingById(long id)
        {
            return await _dbContext.SyncSettings.Include(s => s.Schedule).Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SyncSetting>> GetSyncSettingsByProjectId(long projectId)
        {
            return await _dbContext.SyncSettings.Where(s => s.ProjectId == projectId).ToListAsync();
        }

        public async Task AddSyncSetting(SyncSetting syncSetting)
        {
            var newSchedule = new Schedule();
            _dbContext.Schedules.Add(newSchedule);
            syncSetting.Schedule = newSchedule;
            syncSetting.ScheduleId = newSchedule.Id;
            _dbContext.SyncSettings.Add(syncSetting);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSyncSetting(SyncSetting syncSetting)
        {
            _dbContext.SyncSettings.Update(syncSetting);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSyncSetting(SyncSetting syncSetting)
        {
            _dbContext.SyncSettings.Remove(syncSetting);
            await _dbContext.SaveChangesAsync();
        }
    }

}
