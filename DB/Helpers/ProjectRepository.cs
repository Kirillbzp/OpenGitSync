using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjects();
        Task<Project> GetProjectById(long id);
        Task<Project> CreateProject(Project project);
        Task<Project> UpdateProject(Project project);
        Task<Project> DeleteProject(Project project);
        Task<IEnumerable<Project>> GetProjectsByUserId(string userId);
    }

    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            return await _dbContext.Projects.Include(p => p.Repositories).ToListAsync();
        }

        public async Task<Project> GetProjectById(long id)
        {
            return await _dbContext.Projects.Include(p => p.Repositories).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> CreateProject(Project project)
        {
            project.CreatedAt = DateTime.Now;
            project.UpdatedAt = DateTime.Now;

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

        public async Task<IEnumerable<Project>> GetProjectsByUserId(string userId)
        {
            return await _dbContext.UserProjects.Where(u => u.UserId == userId).Include(p => p.Project).Select(p => p.Project).ToListAsync();
        }
    }

}
