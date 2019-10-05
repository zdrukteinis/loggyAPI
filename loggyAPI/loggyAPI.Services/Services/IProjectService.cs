using System.Collections.Generic;
using loggyAPI.Data.Entities;

namespace loggyAPI.Services.Services
{
    public interface IProjectService
    {
        Project CreateProject(Project project);
        Project UpdateProject(Project projectParam);
        void Delete(Project project);
        Project GetProject(string projectName, User user);
        List<Project> GetUserProjects(User user);
        Project GetProject(int id);
    }
}
