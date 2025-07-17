using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
<<<<<<<< HEAD:DataAccessObjects/UserDAO.cs
        public static async Task<User> GetUserByEmailAndPassword(string email, string password)
========
        private readonly HealthCareSystemContext _context;
        public UserRepository(HealthCareSystemContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
>>>>>>>> develop:Repositories/Repositories/UserRepository.cs
        {
            try
            {
                var _context = new HealthCareSystemContext();
                return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<User> GetUserById(int userId)
        {
            try
            {
                var _context = new HealthCareSystemContext();
                return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<User> CheckUserExist(string email)
        {
            try
            {
                var _context = new HealthCareSystemContext();
                return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task CreateUser(User user)
        {
<<<<<<<< HEAD:DataAccessObjects/UserDAO.cs
            try
            {
                var _context = new HealthCareSystemContext();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
========

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

>>>>>>>> develop:Repositories/Repositories/UserRepository.cs
        }
    }
}
