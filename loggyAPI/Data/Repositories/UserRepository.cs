using System.Collections.Generic;
using System.Linq;
using loggyAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace loggyAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _dataContext.Users
                .Include(x => x.Role);
        }

        public User GetUserByUsername(string username)
        {
            return _dataContext.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Username == username);
        }

        public User GetUserById(int id)
        {
           return _dataContext.Users.Find(id);
        }

        public void AddUser(User user, UserRole role)
        {
            _dataContext.UsersRole.Add(role);
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _dataContext.Users.Update(user);
            _dataContext.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _dataContext.Users.Remove(user);
            _dataContext.UsersRole.Remove(user.Role);
            _dataContext.SaveChanges();
        }
    }
}
