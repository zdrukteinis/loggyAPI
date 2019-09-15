using System.Collections.Generic;
using System.Linq;
using loggyAPI.Data;
using loggyAPI.Data.Entities;
using loggyAPI.Data.Entities.Enums;
using loggyAPI.Services.Helpers;

namespace loggyAPI.Services.Services
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications

        private DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password, string secret)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
            {
                return null;
            }

            user.GetToken(secret);

            return !user.VerifyPasswordHash(password) 
                ? null 
                : user;
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.Users.Any(x => x.Username == user.Username))
            {
                throw new AppException($"Username \"{user.Username}\" is already taken");
            }

            UserServiceHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var role = new UserRole
            {
                Role = Role.User,
                User = user
            };
            user.Role = role;
            _context.UsersRole.Add(role);
            _context.Users.Add(user);
           
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_context.Users.Any(x => x.Username == userParam.Username))
                {
                    throw new AppException("Username " + userParam.Username + " is already taken");
                }
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                UserServiceHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User userParam)
        {
            _context.Users.Remove(userParam);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }
    }
}
