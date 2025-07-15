using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class DoctorDAO
    {
        private static DoctorDAO instance;
        private readonly HealthCareSystemContext _context;
        private static readonly object _lock = new object();
        public static DoctorDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new DoctorDAO();
                        }
                    }
                }
                return instance;
            }
        }
        public DoctorDAO()
        {
            _context = new HealthCareSystemContext();
        }

        public async Task<Doctor> GetDoctorByUserId(int userId)
        {
            try
            {
                return await _context.Doctors.Include(d => d.User).Include(d => d.Specialty).FirstOrDefaultAsync(d => d.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
