using System;
using System.Collections.Generic;
using System.Text;
using loggyAPI.Data.Entities;

namespace loggyAPI.Data.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserByUsername(string username);
        User GetUserById(int id);
        void AddUser(User user, UserRole role);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
