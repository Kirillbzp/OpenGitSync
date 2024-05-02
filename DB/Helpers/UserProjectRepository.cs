using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface IUserProjectRepository
    {
        Task<UserProject> GetUserProjectById(long id);
        Task<IEnumerable<UserProject>> GetUserProjectsByProjectId(long projectId);
        Task<IEnumerable<UserProject>> GetUserProjectsByUserId(string userId);
        Task AddUserProject(UserProject userProject);
        Task UpdateUserProject(UserProject userProject);
        Task DeleteUserProject(UserProject userProject);
    }

    public class UserProjectRepository : IUserProjectRepository
    {
        private readonly AppDbContext _dbContext;

        public UserProjectRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProject> GetUserProjectById(long id)
        {
            return await _dbContext.UserProjects.FindAsync(id);
        }

        public async Task<IEnumerable<UserProject>> GetUserProjectsByProjectId(long projectId)
        {
            return await _dbContext.UserProjects.Where(up => up.ProjectId == projectId).ToListAsync();
        }

        public async Task<IEnumerable<UserProject>> GetUserProjectsByUserId(string userId)
        {
            return await _dbContext.UserProjects.Where(up => up.UserId == userId).ToListAsync();
        }

        public async Task AddUserProject(UserProject userProject)
        {
            _dbContext.UserProjects.Add(userProject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserProject(UserProject userProject)
        {
            _dbContext.UserProjects.Update(userProject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserProject(UserProject userProject)
        {
            _dbContext.UserProjects.Remove(userProject);
            await _dbContext.SaveChangesAsync();
        }
    }

}
