using System.Collections.Generic;
using loggyAPI.Data.Entities;
using loggyAPI.Data.Repositories;

namespace loggyAPI.Services.Services
{
    public class LogService : ILogService
    {
        private ILogRepository _logRepository;
        private IProjectRepository _projectRepository;

        public LogService(ILogRepository logRepository, IProjectRepository projectRepository)
        {
            _logRepository = logRepository;
            _projectRepository = projectRepository;
        }

        public void CreateLogEntry(LogEntry logEntry)
        {
            if (logEntry.Project == null)
            {
                throw new AppException("Project is required");
            }

            if (logEntry.User == null)
            {
                throw new AppException("User is required");
            }

            logEntry.Project = _projectRepository.GetProjectById(logEntry.Project.Id, logEntry.User);
            _logRepository.CreateLogEntry(logEntry);
        }

        public LogEntry UpdateLogEntry(LogEntry logEntry)
        {
            if (logEntry.Project == null)
            {
                throw new AppException("Project is required");
            }

            if (logEntry.User == null)
            {
                throw new AppException("User is required");
            }

            //ad time range check
            var existingLogEntry = _logRepository.GetLogEntryById(logEntry.Id);
            existingLogEntry.From = logEntry.From;
            existingLogEntry.To = logEntry.To;
            existingLogEntry.Description = logEntry.Description;
            _logRepository.UpdateLogEntry(existingLogEntry);

            return existingLogEntry;
        }

        public void DeleteLogEntry(LogEntry logEntry)
        {
            var existingLogEntry = _logRepository.GetLogEntryById(logEntry.Id);

            if (existingLogEntry == null)
            {
                throw new AppException("Log entry not found");
            }

            _logRepository.DeleteLogEntry(existingLogEntry);
        }

        public List<LogEntry> GetProjectEntries(Project project)
        {
            if (project == null)
            {
                throw new AppException("Project is required");
            }

            return  _logRepository.GetProjectLogEntries(project);
        }
    }
}
