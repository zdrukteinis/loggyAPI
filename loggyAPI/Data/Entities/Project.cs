using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace loggyAPI.Data.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<LogEntry> LogEntries { get; set; }
        public User User { get; set; }
    }
}
