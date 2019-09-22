using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using loggyAPI.Data.Entities.Enums;

namespace loggyAPI.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public UserRole Role { get; set; }
        public string Token { get; set; }
    }
}
