using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class UserDAO
    {
        private static UserDAO instance;
        private readonly HealthCareSystemContext _context;
        private static readonly object _lock = new object();
        public static UserDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new UserDAO();
                        }
                    }
                }
                return instance;
            }
        }

        public UserDAO()
        {
            _context = new HealthCareSystemContext();
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
    }
}
