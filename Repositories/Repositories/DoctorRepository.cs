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
        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            return await _context.Doctors.Include(d => d.User)
                                         .Include(d => d.Specialty)
                                         .ToListAsync();
        }

        public async Task<List<Doctor>> GetBySpecialtyAsync(int specialtyId)
        {
            return await _context.Doctors.Include(d => d.User).Where(d => d.SpecialtyId == specialtyId).ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int doctorUserId)
        {
            return await _context.Doctors.Include(d => d.User).Include(d => d.Specialty).FirstOrDefaultAsync(d => d.UserId == doctorUserId);
        }
    }
}
