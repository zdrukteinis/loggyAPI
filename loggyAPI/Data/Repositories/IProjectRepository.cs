using System.Collections.Generic;
using loggyAPI.Data.Entities;

namespace loggyAPI.Data.Repositories
{
    public interface IProjectRepository
    {
        void CreateProject(Project project);
        Project GetProjectByName(string projectName, User user);
        Project GetProjectById(int projectId, User user);
        List<Project> GetUserProjects(User user);
        void UpdateProject(Project project);
        void DeleteProject(Project project);
        void DeleteUserProjects(User user);
    }
}
