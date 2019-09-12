using System.Collections.Generic;
using loggyAPI.Data.Entities;

namespace loggyAPI.Services.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password, string secret);
        IEnumerable<User> GetAll();
    }
}
