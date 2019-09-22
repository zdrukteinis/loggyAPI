using System.Collections.Generic;
using loggyAPI.Data.Entities;
using loggyAPI.Data.Entities.Enums;
using loggyAPI.Data.Repositories;
using loggyAPI.Services.Helpers;

namespace loggyAPI.Services.Services
{
    public class UserService
        : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public UserService(IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public User Authenticate(string username, string password, string secret)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new AppException("Username is required");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new AppException("Password is required");
            }

            var user = _userRepository.GetUserByUsername(username);

            // check if username exists
            if (user == null)
            {
                throw new AppException("User is not registered");
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

            var existingUser = _userRepository.GetUserByUsername(user.Username);

            if (existingUser != null)
            {
                throw new AppException($"Username \"{user.Username}\" is already taken");
            }

            UserServiceHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var role = new UserRole
            {
                Role = Role.User,
            };
            user.Role = role;

            _userRepository.AddUser(user,role);

            return user;
        }

        public User Update(User userParam, string password = null)
        {
            var user = _userRepository.GetUserById(userParam.Id);

            if (user == null)
            {
                throw new AppException("User not found");
            }

            if (userParam.Username != user.Username)
            {
                var existingUser = _userRepository.GetUserByUsername(userParam.Username);

                // username has changed so check if the new username is already taken
                if (existingUser != null)
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

            _userRepository.UpdateUser(user);
            return user;
        }

        public void Delete(User userParam)
        {
            if (userParam == null)
            {
                throw new AppException("User param is null");
            }

            var user = _userRepository.GetUserByUsername(userParam.Username);

            if (user == null)
            {
                throw new AppException("User not found");
            }

            _projectRepository.DeleteUserProjects(user);
            _userRepository.DeleteUser(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetById(int id)
        {
            return _userRepository.GetUserById(id);
        }
    }
}
