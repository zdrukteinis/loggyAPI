using System;
using System.Collections.Generic;
using loggyAPI.Data.Entities;
using loggyAPI.Data.Repositories;
using loggyAPI.Services;
using loggyAPI.Services.Services;
using Moq;
using NUnit.Framework;

namespace loggyAPI.Test
{
    public class LogServiceTests
    {
        private IUserService _userService;
        private Mock<IUserRepository> _userRepository;
        private IProjectService _projectService;
        private Mock<IProjectRepository> _projectRepository;
        private ILogService _logService;
        private Mock<ILogRepository> _logRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _userService = new UserService(_userRepository.Object, null);
            _projectRepository = new Mock<IProjectRepository>();
            _projectService = new ProjectService(_projectRepository.Object, _userRepository.Object);
            _logRepository = new Mock<ILogRepository>();
            _logService = new LogService(_logRepository.Object, _projectRepository.Object);
        }

        [Test]
        public void CreateLogEntry_LogProjectNotProvided_ThrowsAppExceptions()
        {
            var logEntry = new LogEntry
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(7),
                Description = "",
                Project = new Project
                {
                    Id = 1,
                    Name = "ProjectOne",
                    Description = "Project description",
                    User = new User
                    {
                        Id = 1,
                        Username = "Zane"
                    }
                }
            };

            logEntry.Project = null;

            var ex = Assert.Throws(typeof(AppException),
                () => _logService.CreateLogEntry(logEntry));
            Assert.That(ex.Message, Is.EqualTo("Project is required"));
        }

        [Test]
        public void CreateLogEntry_LogOwnerNotProvided_ThrowsAppExceptions()
        {
            var logEntry = new LogEntry
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(7),
                Description = "",
                Project = new Project
                {
                    Id = 1,
                    Name = "ProjectOne",
                    Description = "Project description",
                    User = new User {Id = 1, Username = "Zane"}
                },
                User = null
            };


            var ex = Assert.Throws(typeof(AppException),
                () => _logService.CreateLogEntry(logEntry));
            Assert.That(ex.Message, Is.EqualTo("User is required"));
        }

        [Test]
        public void CreateLogEntry_LogEntryDetailsAreValid_CreatesLogEntry()
        {
            var logEntry = new LogEntry
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(7),
                Description = "",
                Project = new Project
                {
                    Id = 1,
                    Name = "ProjectOne",
                    Description = "Project description",
                    User = new User
                    {
                        Id = 1,
                        Username = "Zane"
                    }
                },
                User = new User
                {
                    Id = 1,
                    Username = "Zane"
                }
            };

            _logService.CreateLogEntry(logEntry);
        }

        [Test]
        public void UpdateLogEntry_LogProjectNotProvided_ThrowsAppExceptions()
        {
            var logEntry = new LogEntry
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(7),
                Description = "",
                Project = null,
                User = new User
                {
                    Id = 1,
                    Username = "Zane"
                }
            };

            var ex = Assert.Throws(typeof(AppException),
                () => _logService.UpdateLogEntry(logEntry));
            Assert.That(ex.Message, Is.EqualTo("Project is required"));
        }

        [Test]
        public void UpdateLogEntry_LogOwnerNotProvided_ThrowsAppExceptions()
        {
            var logEntry = new LogEntry
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(7),
                Description = "",
                Project = new Project
                {
                    Id = 1,
                    Name = "ProjectOne",
                    Description = "Project description",
                    User = new User
                    {
                        Id = 1,
                        Username = "Zane"
                    }
                },
                User = null
            };

            var ex = Assert.Throws(typeof(AppException),
                () => _logService.UpdateLogEntry(logEntry));
            Assert.That(ex.Message, Is.EqualTo("User is required"));
        }

        [Test]
        public void UpdateLogEntry_LogEntryDetailsAreValid_CreatesLogEntry()
        {
            var newLogEntry = new LogEntry
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(7),
                Description = "",
                Project = new Project
                {
                    Id = 1,
                    Name = "ProjectOneNew",
                    Description = "Project descriptionNew",
                    User = new User
                    {
                        Id = 1,
                        Username = "Zane"
                    }
                },
                User = new User
                {
                    Id = 1,
                    Username = "Zane"
                }
            };

            var existingLogEntry = new LogEntry
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(7),
                Description = "",
                Project = new Project
                {
                    Id = 1,
                    Name = "ProjectOne",
                    Description = "Project description",
                    User = new User
                    {
                        Id = 1,
                        Username = "Zane"
                    }
                },
                User = new User
                {
                    Id = 1,
                    Username = "Zane"
                }
            };

            _logRepository.Setup(x => x.GetLogEntryById(newLogEntry.Id))
                .Returns(existingLogEntry);

            existingLogEntry.From = newLogEntry.From;
            existingLogEntry.To = newLogEntry.To;
            existingLogEntry.Description = newLogEntry.Description;

            var updatedLogEntry = _logService.UpdateLogEntry(newLogEntry);

            Assert.AreEqual(existingLogEntry,updatedLogEntry);
        }

        [Test]
        public void DeleteLogEntry_LogEntryNotPresent_ThrowsAppException()
        {
            var existingLogEntry = new LogEntry
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(7),
                Description = "",
                Project = new Project
                {
                    Id = 1,
                    Name = "ProjectOne",
                    Description = "Project description",
                    User = new User
                    {
                        Id = 1,
                        Username = "Zane"
                    }
                },
                User = new User
                {
                    Id = 1,
                    Username = "Zane"
                }
            };

            var ex = Assert.Throws(typeof(AppException),
                () => _logService.DeleteLogEntry(existingLogEntry));
            Assert.That(ex.Message, Is.EqualTo("Log entry not found"));
        }

        [Test]
        public void DeleteLogEntry_LogDeletionDetailsAreValid_DeletesLogEntry()
        {
            var existingLogEntry = new LogEntry
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(7),
                Description = "",
                Project = new Project
                {
                    Id = 1,
                    Name = "ProjectOne",
                    Description = "Project description",
                    User = new User
                    {
                        Id = 1,
                        Username = "Zane"
                    }
                },
                User = new User
                {
                    Id = 1,
                    Username = "Zane"
                }
            };

            _logRepository.Setup(x => x.GetLogEntryById(existingLogEntry.Id))
                .Returns(existingLogEntry);
            _logService.DeleteLogEntry(existingLogEntry);
        }

        [Test]
        public void GetProjectEntries_LogEntriesProjectIsNotProvided_ThrowsAppException()
        {
            var existingProject = new Project
            {
                Id = 1,
                Name = "ProjectOne",
                Description = "Project description",
                User = new User
                {
                    Id = 1,
                    Username = "Zane"
                }
            };

            var ex = Assert.Throws(typeof(AppException),
                () => _logService.GetProjectEntries(null));
            Assert.That(ex.Message, Is.EqualTo("Project is required"));
        }

        [Test]
        public void GetProjectEntries_ProjectHasLogEntries_ReturnsProjectLogEntries()
        {
            var existingProject = new Project
            {
                Id = 1,
                Name = "ProjectOne",
                Description = "Project description",
                User = new User
                {
                    Id = 1,
                    Username = "Zane"
                },
                LogEntries = new List<LogEntry>
                {
                    new LogEntry
                    {
                        From = DateTime.Today,
                        To = DateTime.Today.AddDays(7),
                        Description = "LogEntryOne",
                    },
                    new LogEntry
                    {
                        From = DateTime.Today,
                        To = DateTime.Today.AddDays(7),
                        Description = "LogEntryTwo",
                    }
                }
            };

            _logRepository.Setup(x => x.GetProjectLogEntries(existingProject))
                .Returns(existingProject.LogEntries);

            var retrievedLogEntries = _logService.GetProjectEntries(existingProject);
            Assert.AreEqual(existingProject.LogEntries, retrievedLogEntries);
        }
    }
}
