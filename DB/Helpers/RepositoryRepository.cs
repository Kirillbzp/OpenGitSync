using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface IRepositoryRepository
    {
        Task<Repository> GetRepositoryById(long id);
        Task<IEnumerable<Repository>> GetRepositoriesByProjectId(long projectId);
        Task<Repository> UpdateRepository(Repository repository);
        Task DeleteRepository(Repository repository);
        Task<Repository> CreateRepository(Repository repository);
        Task<IEnumerable<Repository>> GetRepositories();
        Task<IEnumerable<Repository>> RepositoryTypeahead(string query, long projectId);
    }


    public class RepositoryRepository : IRepositoryRepository
    {
        private readonly AppDbContext _dbContext;

        public RepositoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Repository> GetRepositoryById(long id)
        {
            return await _dbContext.Repositories.FindAsync(id);
        }

        public async Task<IEnumerable<Repository>> GetRepositoriesByProjectId(long projectId)
        {
            return await _dbContext.Repositories.Where(r => r.ProjectId == projectId).ToListAsync();
        }

        public async Task<Repository> UpdateRepository(Repository repository)
        {
            repository.UpdatedAt = DateTime.Now;

            _dbContext.Repositories.Update(repository);
            await _dbContext.SaveChangesAsync();

            return repository;
        }

        public async Task DeleteRepository(Repository repository)
        {
            _dbContext.Repositories.Remove(repository);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Repository> CreateRepository(Repository repository)
        {

            repository.CreatedAt = DateTime.Now;
            repository.UpdatedAt = DateTime.Now;

            _dbContext.Repositories.Add(repository);
            await _dbContext.SaveChangesAsync();

            return repository;
        }

        public async Task<IEnumerable<Repository>> GetRepositories()
        {
            return await _dbContext.Repositories.ToListAsync();
        }

        public async Task<IEnumerable<Repository>> RepositoryTypeahead(string query, long projectId)
        {
            return await _dbContext.Repositories.Where(r => r.ProjectId == projectId && r.Name.Contains(query)).ToListAsync();
        }

    }


}
