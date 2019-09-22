using System;
using System.Collections.Generic;
using System.Linq;
using loggyAPI.Data.Entities;

namespace loggyAPI.Data.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly DataContext _dataContext;

        public LogRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void CreateLogEntry(LogEntry logEntry)
        {
            _dataContext.LogsEntries.Add(logEntry);
            _dataContext.SaveChanges();
        }

        public void UpdateLogEntry(LogEntry logEntry)
        {
            _dataContext.LogsEntries.Update(logEntry);
            _dataContext.SaveChanges();

        }

        public LogEntry GetLogEntryById(int logEntryId)
        {
            return _dataContext.LogsEntries.FirstOrDefault(x => x.Id == logEntryId);
        }

        public LogEntry GetLogEntryByDate(DateTime from, DateTime to)
        {
            return _dataContext.LogsEntries
                .FirstOrDefault(x => x.From == from && x.To == to);
        }

        public List<LogEntry> GetProjectLogEntries(Project project)
        {
            throw new NotImplementedException();
        }

        public void DeleteLogEntry(LogEntry logEntry)
        {
            _dataContext.LogsEntries.Remove(logEntry);
            _dataContext.SaveChanges();
        }
    }
}
