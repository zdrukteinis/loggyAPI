using System.Collections.Generic;
using loggyAPI.Data.Entities;

namespace loggyAPI.Services.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password, string secret);
        User Create(User user, string password);
        void Update(User userParam, string password = null);
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Delete(User userParam);
    }
}
