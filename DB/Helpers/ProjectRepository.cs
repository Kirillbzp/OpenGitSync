using DB.Models;
using DB.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjects(string userId);
        Task<Project?> GetProjectById(long id, string userId);
        Task<Project> CreateProject(Project project, string userId);
        Task<Project> UpdateProject(Project project);
        Task<Project> DeleteProject(Project project);
    }

    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Project>> GetProjects(string userId)
        {
            return await _dbContext.Projects.Include(p => p.Repositories)
                                            .Include(p => p.UserProjects)
                                            .Where(p => p.UserProjects.Any(up => up.UserId == userId))
                                            .ToListAsync();
        }

        public async Task<Project?> GetProjectById(long id, string userId)
        {
            return await _dbContext.Projects.Include(p => p.Repositories)
                                            .Include(p => p.UserProjects)
                                            .Where(p => p.Id == id && p.UserProjects.Any(up => up.UserId == userId))
                                            .FirstOrDefaultAsync();
        }

        public async Task<Project> CreateProject(Project project, string userId)
        {
            project.CreatedAt = DateTime.Now;
            project.UpdatedAt = DateTime.Now;

            project.UserProjects = new List<UserProject> { new UserProject { UserId = userId, Role = ProjectPermission.Owner } };

            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateProject(Project project)
        {
            project.UpdatedAt = DateTime.Now;
            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync();
            return project;
        }

        public async Task<Project> DeleteProject(Project project)
        {
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
            return project;
        }

    }

}
