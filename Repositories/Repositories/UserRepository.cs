using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HealthCareSystemContext _context;
        public UserRepository(HealthCareSystemContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<User> GetUserById(int userId)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<User> CheckUserExist(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task CreateUser(User user)
        {

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

        }
    }
}
