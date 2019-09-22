using System.Collections.Generic;
using loggyAPI.Data.Entities;

namespace loggyAPI.Services.Services
{
    public interface ILogService
    {
        void CreateLogEntry(LogEntry logEntry);
        LogEntry UpdateLogEntry(LogEntry logEntry);
        void DeleteLogEntry(LogEntry logEntry);
        List<LogEntry> GetProjectEntries(Project project);
    }
}
