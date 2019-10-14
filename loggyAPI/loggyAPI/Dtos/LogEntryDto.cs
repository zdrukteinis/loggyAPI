using System;

namespace loggyAPI.Dtos
{
    public class LogEntryDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public ProjectDto Project { get; set; }
        public UserDto User { get; set; }
    }
}
