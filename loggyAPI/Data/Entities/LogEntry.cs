using System;
using System.ComponentModel.DataAnnotations;

namespace loggyAPI.Data.Entities
{
    public class LogEntry
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Project Project { get; set; }
        public User User { get; set; }
    }
}
