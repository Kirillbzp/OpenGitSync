﻿using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Helpers
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetProjects();
        Project GetProjectById(long id);
        Project CreateProject(Project project);
        Project UpdateProject(Project project);
        Project DeleteProject(Project project);
    }

    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Project> GetProjects()
        {
            return _dbContext.Projects.Include(p => p.Repositories);
        }

        public Project GetProjectById(long id)
        {
            return _dbContext.Projects.Include(p => p.Repositories).FirstOrDefault(p => p.Id == id);
        }

        public Project CreateProject(Project project)
        {
            _dbContext.Projects.Add(project);
            _dbContext.SaveChanges();
            return project;
        }

        public Project UpdateProject(Project project)
        {
            _dbContext.Entry(project).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return project;
        }

        public Project DeleteProject(Project project)
        {
            _dbContext.Projects.Remove(project);
            _dbContext.SaveChanges();
            return project;
        }
    }

}
