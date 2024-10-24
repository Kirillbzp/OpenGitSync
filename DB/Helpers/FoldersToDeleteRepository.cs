using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface IFoldersToDeleteRepository
    {
        Task<IEnumerable<string>> GetFoldersToDeleteAsync();
        Task SetFolderToDeleteAsync(string path);
    }

    public class FoldersToDeleteRepository : IFoldersToDeleteRepository
    {
        private readonly AppDbContext _dbContext;

        public FoldersToDeleteRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> GetFoldersToDeleteAsync()
        {
            var folders = await _dbContext.FoldersToRemove
                                .Where(f => f.Status == FoldersRemoveStatus.NotStarted)
                                .ToListAsync();

            folders.ForEach(f => f.Status = FoldersRemoveStatus.InProgress);
            await _dbContext.SaveChangesAsync();

            return folders.Select(f => f.Path);
        }

        public async Task SetFolderToDeleteAsync(string path)
        {
            var exists = await _dbContext.FoldersToRemove.AnyAsync(f => f.Path == path);
            if (!exists)
            {
                await _dbContext.FoldersToRemove.AddAsync(new FoldersToRemove(path));
                await _dbContext.SaveChangesAsync();
            }
        }

        
    }
}
