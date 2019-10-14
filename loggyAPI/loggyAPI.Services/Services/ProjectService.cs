using System.Collections.Generic;
using loggyAPI.Data.Entities;
using loggyAPI.Data.Repositories;

namespace loggyAPI.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public Project CreateProject(Project project)
        {
            if (string.IsNullOrEmpty(project.Name))
            {
                throw new AppException("Project name is required");
            }

            project.User = _userRepository.GetUserByUsername(project.User.Username);

            var existingProject = _projectRepository.GetProjectByName(project.Name, project.User);

            if (existingProject != null)
            {
                throw new AppException($"Name \"{project.Name}\" is already taken");
            }

            _projectRepository.CreateProject(project);

            return project;
        }

        public Project UpdateProject(Project projectParam)
        {
            projectParam.User = _userRepository.GetUserByUsername(projectParam.User.Username);
            var project = _projectRepository.GetProjectById(projectParam.Id, projectParam.User);

            if (project == null)
            {
                throw new AppException("Project not found");
            }

            project.Name = projectParam.Name;
            project.Description = projectParam.Description;

            _projectRepository.UpdateProject(project);
            return project;
        }

        public void Delete(Project project)
        {
            if (project == null)
            {
                throw new AppException("Project param is null");
            }

            var existingProject = _projectRepository.GetProjectByName(project.Name, project.User);

            if (existingProject == null)
            {
                throw new AppException("Project not found");
            }

            _projectRepository.DeleteProject(existingProject);
        }

        public Project GetProject(string projectName, User user)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new AppException("Project name is empty");
            }

            if (user == null)
            {
                throw new AppException("User is null");
            }

            return _projectRepository.GetProjectByName(projectName, user);
        }

        public List<Project> GetUserProjects(User user)
        {
            if (user == null)
            {
                throw new AppException("User is null");
            }

            var projects =  _projectRepository.GetUserProjects(user);
            return projects;
        }

        public Project GetProject(int id)
        {
            return _projectRepository.GetProjectById(id);
        }

    }
}
