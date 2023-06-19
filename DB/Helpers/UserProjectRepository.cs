using DB.Models;

namespace DB.Helpers
{
    public interface IUserProjectRepository
    {
        UserProject GetUserProjectById(long id);
        IEnumerable<UserProject> GetUserProjectsByProjectId(long projectId);
        IEnumerable<UserProject> GetUserProjectsByUserId(long userId);
        void AddUserProject(UserProject userProject);
        void UpdateUserProject(UserProject userProject);
        void DeleteUserProject(UserProject userProject);
    }

    public class UserProjectRepository : IUserProjectRepository
    {
        private readonly AppDbContext _dbContext;

        public UserProjectRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserProject GetUserProjectById(long id)
        {
            return _dbContext.UserProjects.Find(id);
        }

        public IEnumerable<UserProject> GetUserProjectsByProjectId(long projectId)
        {
            return _dbContext.UserProjects.Where(up => up.ProjectId == projectId).ToList();
        }

        public IEnumerable<UserProject> GetUserProjectsByUserId(long userId)
        {
            return _dbContext.UserProjects.Where(up => up.UserId == userId).ToList();
        }

        public void AddUserProject(UserProject userProject)
        {
            _dbContext.UserProjects.Add(userProject);
        }

        public void UpdateUserProject(UserProject userProject)
        {
            _dbContext.UserProjects.Update(userProject);
        }

        public void DeleteUserProject(UserProject userProject)
        {
            _dbContext.UserProjects.Remove(userProject);
        }
    }

}
