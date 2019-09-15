using loggyAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace loggyAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UsersRole { get; set; }
    }
}
