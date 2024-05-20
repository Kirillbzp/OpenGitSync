using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface IRepositoryRepository
    {
        Task<RepositoryModel?> GetRepositoryById(long id, string userId);
        Task<IEnumerable<RepositoryModel>> GetRepositoriesByProjectId(long projectId, string userId);
        Task<RepositoryModel> UpdateRepository(RepositoryModel repository);
        Task DeleteRepository(RepositoryModel repository);
        Task<RepositoryModel> CreateRepository(RepositoryModel repository);
        Task<IEnumerable<RepositoryModel>> GetRepositories(string userId);
        Task<IEnumerable<RepositoryModel>> RepositoryTypeahead(string query, long projectId, string userId);
    }


    public class RepositoryRepository : IRepositoryRepository
    {
        private readonly AppDbContext _dbContext;

        public RepositoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RepositoryModel?> GetRepositoryById(long id, string userId)
        {
            return await _dbContext.Repositories.Include(r => r.Project)
                                                .ThenInclude(p => p.UserProjects)
                                                .Where(r => r.Id == id && r.Project.UserProjects.Any(up => up.UserId == userId))
                                                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RepositoryModel>> GetRepositoriesByProjectId(long projectId, string userId)
        {
            return await _dbContext.Repositories.Include(r => r.Project)
                                                .ThenInclude(p => p.UserProjects)
                                                .Where(r => r.ProjectId == projectId && r.Project.UserProjects.Any(up => up.UserId == userId))
                                                .ToListAsync();
        }

        public async Task<RepositoryModel> UpdateRepository(RepositoryModel repository)
        {
            repository.UpdatedAt = DateTime.Now;

            _dbContext.Repositories.Update(repository);
            await _dbContext.SaveChangesAsync();

            return repository;
        }

        public async Task DeleteRepository(RepositoryModel repository)
        {
            _dbContext.Repositories.Remove(repository);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RepositoryModel> CreateRepository(RepositoryModel repository)
        {

            repository.CreatedAt = DateTime.Now;
            repository.UpdatedAt = DateTime.Now;

            _dbContext.Repositories.Add(repository);
            await _dbContext.SaveChangesAsync();

            return repository;
        }

        public async Task<IEnumerable<RepositoryModel>> GetRepositories(string userId)
        {
            return await _dbContext.Repositories.Include(r => r.Project)
                                                .ThenInclude(p => p.UserProjects)
                                                .Where(r => r.Project.UserProjects.Any(up => up.UserId == userId))
                                                .ToListAsync();
        }

        public async Task<IEnumerable<RepositoryModel>> RepositoryTypeahead(string query, long projectId, string userId)
        {
            return await _dbContext.Repositories.Include(r => r.Project)
                                                .ThenInclude(p => p.UserProjects)
                                                .Where(r => r.ProjectId == projectId && 
                                                            r.Name.Contains(query) && 
                                                            r.Project.UserProjects.Any(up => up.UserId == userId))
                                                .ToListAsync();
        }

    }


}
