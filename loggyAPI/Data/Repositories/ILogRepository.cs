using System;
using System.Collections.Generic;
using loggyAPI.Data.Entities;

namespace loggyAPI.Data.Repositories
{
    public interface ILogRepository
    {
        void CreateLogEntry(LogEntry logEntry);
        void UpdateLogEntry(LogEntry logEntry);
        LogEntry GetLogEntryById(int logEntryId);
        LogEntry GetLogEntryByDate(DateTime from, DateTime to);
        List<LogEntry> GetProjectLogEntries(Project project);
        void DeleteLogEntry(LogEntry logEntry);
    }
}