using DB.Models;

namespace DB.Helpers
{
    public interface IRepositoryRepository
    {
        Repository GetRepositoryById(long id);
        IEnumerable<Repository> GetRepositoriesByProjectId(long projectId);
        Repository AddRepository(Repository repository);
        Repository UpdateRepository(Repository repository);
        void DeleteRepository(Repository repository);
        Repository CreateRepository(string name, string url);
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

        public Repository AddRepository(Repository repository)
        {
            _dbContext.Repositories.Add(repository);

            return repository;
        }

        public Repository UpdateRepository(Repository repository)
        {
            _dbContext.Repositories.Update(repository);

            return repository;
        }

        public void DeleteRepository(Repository repository)
        {
            _dbContext.Repositories.Remove(repository);
        }

        public Repository CreateRepository(string name, string url)
        {
            var repository = new Repository
            {
                Name = name,
                Url = url
            };

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
