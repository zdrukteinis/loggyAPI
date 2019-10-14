using System.Collections.Generic;
using loggyAPI.Data.Entities;
using loggyAPI.Data.Repositories;
using loggyAPI.Services;
using loggyAPI.Services.Services;
using Moq;
using NUnit.Framework;

namespace loggyAPI.Test
{
    public class ProjectServiceTests
    {
        private IUserService _userService;
        private Mock<IUserRepository> _userRepository;
        private IProjectService _projectService;
        private Mock<IProjectRepository> _projectRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _userService = new UserService(_userRepository.Object, null);
            _projectRepository = new Mock<IProjectRepository>();
            _projectService = new ProjectService(_projectRepository.Object, _userRepository.Object);
        }

        [Test]
        public void CreateProject_ProjectNameMissing_ThrowsAppException()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = null,
                Description = "Awesome project"
            };


            var ex = Assert.Throws(typeof(AppException),
                () => _projectService.CreateProject(existingProject));
            Assert.That(ex.Message, Is.EqualTo("Project name is required"));
        }

        [Test]
        public void CreateProject_ProjectWithSuchNameIsAlreadyCreated_ThrowsAppException()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = "ProjectOne",
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username))
                .Returns(existingProject.User);

            _projectRepository.Setup(x => x.GetProjectByName(existingProject.Name, existingProject.User))
                .Returns(existingProject);

            var ex = Assert.Throws(typeof(AppException),
                () => _projectService.CreateProject(existingProject));
            Assert.That(ex.Message, Is.EqualTo($"Name \"{existingProject.Name}\" is already taken"));
        }

        [Test]
        public void CreateProject_ProjectIsNewAndAllFieldsAreValid_ReturnsCreatedProject()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = "ProjectOne",
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            _projectRepository.Setup(x => x.GetProjectByName(existingProject.Name, existingProject.User))
                .Returns(existingProject);

            var project =  _projectService.CreateProject(existingProject);
            Assert.AreEqual(project, existingProject);
        }

        [Test]
        public void UpdateProject_ProjectIsNotPresent_ThrowsAppException()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = null,
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username)).Returns(new User
            {
                Username = "Jake",
                Id = 1
            });

            var ex = Assert.Throws(typeof(AppException),
                () => _projectService.UpdateProject(existingProject));
            Assert.That(ex.Message, Is.EqualTo("Project not found"));
        }

        [Test]
        public void UpdateProject_ProjectFieldsAreValid_ReturnsCreatedProject()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = "ProjectOne",
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            var projectParam = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = "ProjectOne",
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake",
                    Id = 1
                }
            };

            _projectRepository.Setup(x => x.GetProjectById(projectParam.Id, projectParam.User))
                .Returns(existingProject);

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username))
                .Returns(projectParam.User);

            existingProject.Name = projectParam.Name;
            existingProject.Description = projectParam.Description;

            var createdProject =  _projectService.UpdateProject(projectParam);
            Assert.AreEqual(existingProject, createdProject);
        }

        [Test]
        public void Delete_ProjectParamIsNotProvided_ThrowsAppException()
        {
            var ex = Assert.Throws(typeof(AppException),
                () => _projectService.Delete(null));
            Assert.That(ex.Message, Is.EqualTo("Project param is null"));
        }

        [Test]
        public void Delete_ProjectIsNotPresent_ThrowsAppException()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = null,
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username)).Returns(new User
            {
                Username = "Jake",
                Id = 1
            });

            _projectRepository.Setup(x => x.GetProjectById(existingProject.Id))
                .Returns(existingProject);

            var ex = Assert.Throws(typeof(AppException),
                () => _projectService.Delete(existingProject));
            Assert.That(ex.Message, Is.EqualTo("Project not found"));
        }

        [Test]
        public void Delete_ProvidedProjectDeletionIsValid_DeletesProject()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = null,
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username)).Returns(new User
            {
                Username = "Jake",
                Id = 1
            });

            _projectRepository.Setup(x => x.GetProjectByName(existingProject.Name, existingProject.User))
                .Returns(existingProject);

            _projectService.Delete(existingProject);
        }

        [Test]
        public void GetProject_ProjectNameIsNotProvided_ThrowsException()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = null,
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username)).Returns(new User
            {
                Username = "Jake",
                Id = 1
            });

            _projectRepository.Setup(x => x.GetProjectById(existingProject.Id)).Returns(existingProject);

            var ex = Assert.Throws(typeof(AppException),
                () => _projectService.GetProject(existingProject.Name, null));
            Assert.That(ex.Message, Is.EqualTo("Project name is empty"));
        }

        [Test]
        public void  GetProject_UserIsNotPresent_ThrowsAppException()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = "ProjectOne",
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username)).Returns(new User
            {
                Username = "Jake",
                Id = 1
            });

            var ex = Assert.Throws(typeof(AppException),
                () => _projectService.GetProject(existingProject.Name,null));
            Assert.That(ex.Message, Is.EqualTo("User is null"));
        }

        [Test]
        public void GetProject_RequestedProjectExists_ReturnsProject()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = "ProjectOne",
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake",
                    Id = 1
                }
            };

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username)).Returns(new User
            {
                Username = "Jake",
                Id = 1
            });

            _projectRepository.Setup(x => x.GetProjectByName(existingProject.Name, existingProject.User))
                .Returns(existingProject);

            var retrievedProject = _projectService.GetProject(existingProject.Name, existingProject.User);
            Assert.AreEqual(existingProject, retrievedProject);
        }

        [Test]
        public void GetUserProjects_UserIsNotPresent_ThrowsAppExceptions()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = "ProjectOne",
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username)).Returns(new User
            {
                Username = "Jake",
                Id = 1
            });

            var ex = Assert.Throws(typeof(AppException),
                () => _projectService.GetUserProjects(null));
            Assert.That(ex.Message, Is.EqualTo("User is null"));
        }

        [Test]
        public void GetUserProjects_UserIsPresentAndUserHasProjects_ThrowsAppExceptions()
        {
            var existingUserProjects = new List<Project>
            {
                new Project
                {
                    Description = "ProjectOne"
                },
                new Project
                {
                    Description = "ProjectTwo"
                }
            };

            var userParam = new User
            {
                Username = "Jake"
            };

            _projectRepository.Setup(x => x.GetUserProjects(userParam))
                .Returns(existingUserProjects);


            var retrievedProjects = _projectService.GetUserProjects(userParam);
            Assert.AreEqual(existingUserProjects, retrievedProjects);
        }

        [Test]
        public void GetProject_ProjectWithSuchIdExists_ReturnsProject()
        {
            var existingProject = new Project
            {
                Id = 1,
                LogEntries = new List<LogEntry>(),
                Name = "ProjectOne",
                Description = "Awesome project",
                User = new User
                {
                    Username = "Jake"
                }
            };

            _userRepository.Setup(x => x.GetUserByUsername(existingProject.User.Username)).Returns(new User
            {
                Username = "Jake",
                Id = 1
            });

            _projectRepository.Setup(x => x.GetProjectById(existingProject.Id)).Returns(existingProject);

           var project = _projectService.GetProject(existingProject.Id);

           Assert.AreEqual(existingProject,project);
        }

    }
}
