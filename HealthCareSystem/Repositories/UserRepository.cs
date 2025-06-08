using HealthCareSystem.Models;
using HealthCareSystem.Services;

namespace HealthCareSystem.Repositories
{
    public class UserRepository : IUserService
    {
        private readonly HealthCareSystemContext _context;
        public UserRepository(HealthCareSystemContext context)
        {
            _context = context;
        }
        public User GetUserByAccount(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        }
        public void CreateUser(User user)
        {
            if (user != null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
        }
        public bool IsUserExists(string email)
        {
            return _context.Users.Any(u => u.Email.Equals(email));
        }
    }
}
