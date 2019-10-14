using System;
using System.Collections.Generic;
using loggyAPI.Data.Entities;
using loggyAPI.Data.Entities.Enums;
using loggyAPI.Services.Helpers;
using NUnit.Framework;

namespace loggyAPI.Test
{
    [TestFixture]
    public class UserServiceHelperTests
    {
        [Test]
        public void VerifyPasswordHash_PasswordIsNull_ThrowsArgumentNullException()
        {
            var role = new UserRole
            {
                Id = 1,
                Role = Role.Admin
            };

            var user = new User
            {
                Id = 1,
                Username = "Jake",
                FirstName = "Jacob",
                LastName = "Crea",
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { },
                Role = new UserRole
                {
                    Id = role.Id,
                    Role = role.Role
                }
            };

            var ex = Assert.Throws(typeof(ArgumentNullException),
                () => user.VerifyPasswordHash(null));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: password"));
        }

        [Test]
        public void VerifyPasswordHash_PasswordIsEmpty_ThrowsArgumentException()
        {
            var user = new User
            {
                Id = 1,
                Username = "Jake",
                FirstName = "Jacob",
                LastName = "Crea",
                PasswordHash = new byte[] { },
                PasswordSalt = new byte[] { },
                Role = new UserRole
                {
                    Id = 1,
                    Role = Role.Admin
                }
            };

            var ex = Assert.Throws(typeof(ArgumentException),
                () => user.VerifyPasswordHash(""));
            Assert.That(
                ex.Message,
                Is.EqualTo("Value cannot be empty or whitespace only string.\r\nParameter name: password"));
        }


        [Test]
        public void VerifyPasswordHash_PasswordHashWrongSize_ThrowsArgumentException()
        {
            var user = new User
            {
                Id = 1,
                Username = "Jake",
                FirstName = "Jacob",
                LastName = "Crea",
                Role = new UserRole
                {
                    Id = 1,
                    Role = Role.Admin
                }
            };

            var passwordHashList = new List<byte>();

            for (var i = 0; i < 64; i++)
            {
                passwordHashList.Add(0);
            }

            passwordHashList.RemoveRange(0,2);

            user.PasswordHash = passwordHashList.ToArray();

            var ex = Assert.Throws(typeof(ArgumentException),
                () => user.VerifyPasswordHash("password"));
            Assert.That(
                ex.Message,
                Is.EqualTo("Invalid length of password hash (64 bytes expected).\r\nParameter name: passwordHash"));
        }

        [Test]
        public void VerifyPasswordHash_PasswordSaltWrongSize_ThrowsArgumentException()
        {
            var user = new User
            {
                Id = 1,
                Username = "Jake",
                FirstName = "Jacob",
                LastName = "Crea",
                Role = new UserRole
                {
                    Id = 1,
                    Role = Role.Admin
                }
            };

            var passwordHashList = new List<byte>();

            for (var i = 0; i < 64; i++)
            {
                passwordHashList.Add(0);
            }

            var passwordSaltList = new List<byte>();

            for (var i = 0; i < 128; i++)
            {
                passwordSaltList.Add(0);
            }

            user.PasswordHash = passwordHashList.ToArray();

            passwordSaltList.RemoveRange(0,5);
            user.PasswordSalt = passwordSaltList.ToArray();
            
            var ex = Assert.Throws(typeof(ArgumentException),
                () => user.VerifyPasswordHash("password"));
            Assert.That(
                ex.Message,
                Is.EqualTo("Invalid length of password salt (128 bytes expected).\r\nParameter name: passwordSalt"));
        }

        [Test]
        public void VerifyPasswordHash_PasswordCorrect_ThrowsArgumentException()
        {
            var user = new User
            {
                Id = 1,
                Username = "Jake",
                FirstName = "Jacob",
                LastName = "Crea",
                Role = new UserRole
                {
                    Id = 1,
                    Role = Role.Admin
                }
            };

            var passwordHashList = new List<byte>();

            for (var i = 0; i < 64; i++)
            {
                passwordHashList.Add(0);
            }

            var passwordSaltList = new List<byte>();

            for (var i = 0; i < 128; i++)
            {
                passwordSaltList.Add(0);
            }

            user.PasswordHash = passwordHashList.ToArray();
            user.PasswordSalt = passwordSaltList.ToArray();

            Assert.AreEqual(false, user.VerifyPasswordHash("password"));
        }

        [Test]
        public void CreatePasswordHash_PasswordIsNull_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws(typeof(ArgumentNullException),
                () => UserServiceHelper.CreatePasswordHash(null, out var passwordHash, out var passwordSalt));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: password"));
        }

        [Test]
        public void CreatePasswordHash_PasswordIsEmpty_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws(typeof(ArgumentException),
                () => UserServiceHelper.CreatePasswordHash("", out var passwordHash, out var passwordSalt));
            Assert.That(ex.Message,
                Is.EqualTo("Value cannot be empty or whitespace only string.\r\nParameter name: password"));
        }

        [Test]
        public void CreatePasswordHash_PasswordIsValid_ThrowsArgumentNullException()
        {
            UserServiceHelper.CreatePasswordHash("password", out var passwordHash, out var passwordSalt);
            Assert.AreEqual(64, passwordHash.Length);
            Assert.AreEqual(128, passwordSalt.Length);
        }
    }
}
