using System.ComponentModel.DataAnnotations;
using loggyAPI.Data.Entities.Enums;

namespace loggyAPI.Data.Entities
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public Role Role { get; set; }
    }
}
