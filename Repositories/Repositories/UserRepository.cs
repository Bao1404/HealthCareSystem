using BusinessObjects;
using DataAccessObjects;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetUserByEmailAndPassword(string email, string password) => await UserDAO.Instance.GetUserByEmailAndPassword(email, password);
        public async Task<User> GetUserById(int userId) => await UserDAO.Instance.GetUserById(userId);
    }
}
