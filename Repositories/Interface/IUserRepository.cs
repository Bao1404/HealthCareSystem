using BusinessObjects;

namespace Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAndPassword(string email, string password);
        Task<User> GetUserById(int userId);
        Task<User> CheckUserExist(string email);
        Task CreateUser(User user);
    }
}
