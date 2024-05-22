using DB.Models;
using DB.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface ISyncSettingRepository
    {
        Task<SyncSetting?> GetSyncSettingById(long id, string userId);
        Task<IEnumerable<SyncSetting>> GetSyncSettingsByProjectId(long projectId, string userId);
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

        public async Task<SyncSetting?> GetSyncSettingById(long id, string userId)
        {
            return await _dbContext.SyncSettings.Include(s => s.Schedule)
                                                .Include(s => s.Project)
                                                .ThenInclude(p => p.UserProjects)
                                                .Where(s => s.Id == id && s.Project.UserProjects.Any(up => up.UserId == userId))
                                                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SyncSetting>> GetSyncSettingsByProjectId(long projectId, string userId)
        {
            return await _dbContext.SyncSettings.Include(s => s.Project)
                                                .ThenInclude(p => p.UserProjects)
                                                .Where(s => s.ProjectId == projectId && s.Project.UserProjects.Any(up => up.UserId == userId))
                                                .ToListAsync();
        }

        public async Task AddSyncSetting(SyncSetting syncSetting)
        {
            var newSchedule = new Schedule();
            _dbContext.Schedules.Add(newSchedule);
            syncSetting.Schedule = newSchedule;
            syncSetting.ScheduleId = newSchedule.Id;
            syncSetting.SyncStartType = SyncStartTypes.Manually;
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
