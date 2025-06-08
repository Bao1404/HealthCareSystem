using HealthCareSystem.Models;

namespace HealthCareSystem.Services
{
    public interface IUserService
    {
        public User GetUserByAccount(string email, string password);
        public bool IsUserExists(string email);
        public void CreateUser(User user);
    }
}
