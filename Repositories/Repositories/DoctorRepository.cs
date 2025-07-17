using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthCareSystemContext _context;
        public DoctorRepository(HealthCareSystemContext context)
        {
            _context = context;
        }

        public async Task<Doctor> GetDoctorsByIdAsync(int userId)
        {
            try
            {
                return await _context.Doctors.Include(d => d.User).Include(d => d.Specialty).Include(d => d.Appointments).FirstOrDefaultAsync(d => d.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Doctor>> SearchDoctorsAsync(string fullName, string phoneNumber, string email, string specialtyName)
        {
            try
            {
                var query = _context.Doctors.Include(d => d.User)
                                             .Include(d => d.Specialty)
                                             .AsQueryable();

                if (!string.IsNullOrEmpty(fullName))
                {
                    query = query.Where(d => d.User.FullName.Contains(fullName));
                }

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    query = query.Where(d => d.User.PhoneNumber.Contains(phoneNumber));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    query = query.Where(d => d.User.Email.Contains(email));
                }

                if (!string.IsNullOrEmpty(specialtyName))
                {
                    query = query.Where(d => d.Specialty.Name.Contains(specialtyName));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
