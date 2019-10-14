using System.Collections.Generic;
using System.Linq;
using loggyAPI.Data.Entities;
using loggyAPI.Data.Repositories;
using loggyAPI.Services;
using loggyAPI.Services.Helpers;
using loggyAPI.Services.Services;
using Moq;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        private IUserService _userService;
        private Mock<IUserRepository> _userRepository;
        private Mock<IProjectRepository> _projectRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _projectRepository = new Mock<IProjectRepository>();
            _userService = new UserService(_userRepository.Object, _projectRepository.Object);
        }

        [Test]
        public void Authenticate_UsernameIsNull_ThrowsAppException()
        {
            var ex = Assert.Throws(typeof(AppException),
                () => _userService.Authenticate("", "", ""));
            Assert.That(ex.Message, Is.EqualTo("Username is required"));
        }

        [Test]
        public void Authenticate_PasswordIsNull_ThrowsAppException()
        {
            var ex = Assert.Throws(typeof(AppException),
                () => _userService.Authenticate("username", "", ""));
            Assert.That(ex.Message, Is.EqualTo("Password is required"));
        }

        [Test]
        public void Authenticate_UserIsNotInDb_ThrowsAppException()
        {
            _userRepository
                .Setup(x => x.GetUserByUsername("username"))
                .Returns((User) null);

            var ex = Assert.Throws(typeof(AppException),
                () => _userService.Authenticate("username", "username", ""));
            Assert.That(ex.Message, Is.EqualTo("User is not registered"));
        }

        [Test]
        public void Authenticate_UserIsInDb_ReturnsUserEntity()
        {
            var user = new User
            {
                Username = "username",
                Role = new UserRole
                {
                    Role = loggyAPI.Data.Entities.Enums.Role.Admin
                }
            };

            UserServiceHelper.CreatePasswordHash("username", out var hash, out var salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            _userRepository
                .Setup(x => x.GetUserByUsername("username"))
                .Returns(user);

            var result = _userService.Authenticate("username", "username", "111dafasfasfasfasfasfsd");

            Assert.AreEqual(true, result != null);
        }

        [Test]
        public void Create_UsernameIsNull_ThrowsAppException()
        {
            var ex = Assert.Throws(typeof(AppException),
                () => _userService.Create(new User(),""));
            Assert.That(ex.Message, Is.EqualTo("Password is required"));
        }

        [Test]
        public void Create_UsernameIsAlreadyTaken_ThrowsAppException()
        {
            var user = new User
            {
                Username = "username"
            };

            _userRepository
                .Setup(x => x.GetUserByUsername("username"))
                .Returns(user);

            var ex = Assert.Throws(typeof(AppException),
                () => _userService.Create(user, "password"));
            Assert.That(ex.Message, Is.EqualTo("Username \"username\" is already taken"));
        }

        [Test]
        public void Create_UserDtoIsValid_ReturnsUser()
        {
            var user = new User
            {
                Username = "username"
            };

            _userRepository
                .Setup(x => x.GetUserByUsername("username"))
                .Returns((User) null);

            var createdUser = _userService.Create(user, "password");
            Assert.AreEqual(true, createdUser != null);
        }

        [Test]
        public void Update_UserIsNotPresentInDb_ThrowsAppException()
        {
            var ex = Assert.Throws(typeof(AppException),
                () => _userService.Update(new User(), "password"));
            Assert.That(ex.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public void Update_UsernameIsAlreadyTaken_ThrowsAppException()
        {
            var oldUser = new User
            {
                Username = "username",
                Id = 1
            };

            var newUser = new User
            {
                Username = "username2",
                Id = 1
            };

            _userRepository
                .Setup(x => x.GetUserByUsername("username2"))
                .Returns(newUser);
            _userRepository
                .Setup(x => x.GetUserById(oldUser.Id))
                .Returns(oldUser);

            var ex = Assert.Throws(typeof(AppException),
                () => _userService.Update(newUser, "password"));
            Assert.That(ex.Message, Is.EqualTo($"Username {newUser.Username} is already taken"));
        }

        [Test]
        public void Update_UsernameIsNewButNotTaken_ReturnsUpdatedUser()
        {
            var oldUser = new User
            {
                Username = "username",
                Id = 1
            };

            var newUser = new User
            {
                Username = "username2",
                Id = 1
            };

            _userRepository
                .Setup(x => x.GetUserById(oldUser.Id))
                .Returns(oldUser);

            var updatedUser = _userService.Update(newUser, "password");

            oldUser.FirstName = newUser.FirstName;
            oldUser.LastName = newUser.LastName;
            oldUser.Username = newUser.Username;

            Assert.AreEqual(oldUser,updatedUser);
        }

        [Test]
        public void Update_UsernameDtoIsValid_ReturnsUpdatedUser()
        {
            var oldUser = new User
            {
                Username = "username",
                Id = 1
            };

            var newUser = new User
            {
                Username = "username2",
                Id = 1
            };

            _userRepository
                .Setup(x => x.GetUserByUsername("username2"))
                .Returns(oldUser);
            _userRepository
                .Setup(x => x.GetUserById(oldUser.Id))
                .Returns(newUser);

            _userService.Update(newUser, "password");
        }

        [Test]
        public void Delete_UserParamIsNull_ThrowsAppException()
        {
            var user = new User
            {
                Username = "username",
                Id = 1
            };

            _userRepository
                .Setup(x => x.GetUserByUsername("username2"))
                .Returns(user);
            _userRepository
                .Setup(x => x.GetUserById(user.Id))
                .Returns(user);

            var ex = Assert.Throws(typeof(AppException),
                () => _userService.Delete(null));
            Assert.That(ex.Message, Is.EqualTo("User param is null"));
        }

        [Test]
        public void Update_UserDtoIsValid_DeletesUser()
        {
            var user = new User
            {
                Username = "username",
                Id = 1
            };

            _userRepository
                .Setup(x => x.GetUserByUsername("username"))
                .Returns(user);
            _userRepository
                .Setup(x => x.GetUserById(user.Id))
                .Returns(user);

            _userService.Delete(user);
        }

        [Test]
        public void Delete_UserIsNotPresent_ThrowsAppException()
        {
            var user = new User
            {
                Username = "username",
                Id = 1
            };

            _userRepository
                .Setup(x => x.GetUserById(user.Id))
                .Returns(user);

            var ex = Assert.Throws(typeof(AppException),
                () => _userService.Delete(user));
            Assert.That(ex.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public void GetAll_UserDtoIsValid_ReturnsUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Username = "username"
                }
            };

            _userRepository
                .Setup(x => x.GetAllUsers())
                .Returns(users.AsEnumerable());

           var retrievedUsers = _userService.GetAll();
           Assert.AreEqual(users.AsEnumerable(), retrievedUsers);
        }

        [Test]
        public void GetById_UserDtoIsValid_ReturnsUser()
        {
            var user = new User
            {
                Username = "username",
                Id = 1
            };

            _userRepository
                .Setup(x => x.GetUserById(user.Id))
                .Returns(user);

            var retrievedUser = _userService.GetById(user.Id);
            Assert.AreEqual(user, retrievedUser);
        }
    }
}