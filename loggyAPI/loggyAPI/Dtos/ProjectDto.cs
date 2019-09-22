using System.Collections.Generic;

namespace loggyAPI.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<LogEntryDto> LogEntries { get; set; }
    }
}
