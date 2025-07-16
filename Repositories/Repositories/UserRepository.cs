using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> GetUserByEmailAndPassword(string email, string password) => UserDAO.GetUserByEmailAndPassword(email, password);
        public Task<User> GetUserById(int userId) => UserDAO.GetUserById(userId);
        public Task<User> CheckUserExist(string email) => UserDAO.CheckUserExist(email);
        public Task CreateUser(User user) => UserDAO.CreateUser(user);
    }   
}
