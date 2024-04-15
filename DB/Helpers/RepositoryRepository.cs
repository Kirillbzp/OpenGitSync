using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface IRepositoryRepository
    {
        Repository GetRepositoryById(long id);
        IEnumerable<Repository> GetRepositoriesByProjectId(long projectId);
        Repository UpdateRepository(Repository repository);
        void DeleteRepository(Repository repository);
        Repository CreateRepository(Repository repository);
        IEnumerable<Repository> GetRepositories();
    }


    public class RepositoryRepository : IRepositoryRepository
    {
        private readonly AppDbContext _dbContext;

        public RepositoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Repository GetRepositoryById(long id)
        {
            return _dbContext.Repositories.Find(id);
        }

        public IEnumerable<Repository> GetRepositoriesByProjectId(long projectId)
        {
            return _dbContext.Repositories.Where(r => r.ProjectId == projectId).ToList();
        }

        public Repository UpdateRepository(Repository repository)
        {
            repository.UpdatedAt = DateTime.Now;

            _dbContext.Repositories.Update(repository);
            _dbContext.SaveChanges();

            return repository;
        }

        public void DeleteRepository(Repository repository)
        {
            _dbContext.Repositories.Remove(repository);
            _dbContext.SaveChanges();
        }

        public Repository CreateRepository(Repository repository)
        {

            repository.CreatedAt = DateTime.Now;
            repository.UpdatedAt = DateTime.Now;

            _dbContext.Repositories.Add(repository);
            _dbContext.SaveChanges();

            return repository;
        }

        public IEnumerable<Repository> GetRepositories()
        {
            return _dbContext.Repositories.ToList();
        }

    }


}
