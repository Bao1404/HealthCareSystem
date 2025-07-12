using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAndPassword(string email, string password);
        Task<User> GetUserById(int userId);
        Task<User> CheckUserExist(string email);
        Task CreateUser(User user);
    }
}
