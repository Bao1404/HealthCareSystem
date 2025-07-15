using BusinessObjects;
using Repositories;
using Repositories.Interface;
using Repositories.Repositories;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService()
        {
            _userRepository = new UserRepository();
        }
        public Task<User> GetUserByEmailAndPassword(string email, string password) => _userRepository.GetUserByEmailAndPassword(email, password);
        public Task<User> GetUserById(int userId) => _userRepository.GetUserById(userId);
        public Task<User> CheckUserExist(string email) => _userRepository.CheckUserExist(email);
        public Task CreateUser(User user) => _userRepository.CreateUser(user);
    }
}
