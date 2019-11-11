using System.Collections.Generic;
using System.Linq;
using loggyAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace loggyAPI.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _dataContext;

        public ProjectRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void CreateProject(Project project)
        {
            _dataContext.Projects.Add(project);
            _dataContext.SaveChanges();
        }

        public Project GetProjectByName(string projectName, User user)
        {
            return _dataContext.Projects
                .FirstOrDefault(x => x.Name == projectName && x.User.Username == user.Username);
        }

        public Project GetProjectById(int projectId, User user)
        {
            return _dataContext.Projects
                .FirstOrDefault(x => x.Id == projectId);
        }

        public List<Project> GetUserProjects(User user)
        {
            return _dataContext.Projects
                .Where(x => x.User.Username == user.Username)
                .ToList();
        }

        public void UpdateProject(Project project)
        {
            _dataContext.Projects.Update(project);
            _dataContext.SaveChanges();
        }

        public void DeleteProject(Project project)
        {
            _dataContext.Projects.Remove(project);
            _dataContext.SaveChanges();
        }

        public void DeleteUserProjects(User user)
        {
            var projectsToRemove = _dataContext.Projects
                .Include(x => x.LogEntries)
                .Where(x => x.User.Id == user.Id).ToList();

            if (projectsToRemove.Count == 0)
            {
                return;
            }

            _dataContext.Remove(projectsToRemove);
            _dataContext.SaveChanges();
        }

        public Project GetProjectById(int projectId)
        {
            return _dataContext.Projects
                .Include(x => x.LogEntries)
                .FirstOrDefault(x => x.Id == projectId);
        }
    }
}
