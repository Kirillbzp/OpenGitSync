namespace DB.Helpers
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IRepositoryRepository RepositoryRepository { get; }
        ISyncSettingRepository SyncSettingRepository { get; }
        IUserProjectRepository UserProjectRepository { get; }

        void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private bool _disposed;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _disposed = false;
            UserRepository = new UserRepository(_dbContext);
            ProjectRepository = new ProjectRepository(_dbContext);
            RepositoryRepository = new RepositoryRepository(_dbContext);
            SyncSettingRepository = new SyncSettingRepository(_dbContext);
            UserProjectRepository = new UserProjectRepository(_dbContext);
        }

        public IUserRepository UserRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IRepositoryRepository RepositoryRepository { get; }
        public ISyncSettingRepository SyncSettingRepository { get; }
        public IUserProjectRepository UserProjectRepository { get; }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
