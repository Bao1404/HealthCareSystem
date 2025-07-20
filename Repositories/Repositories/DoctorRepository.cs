using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthCareSystemContext _context;
        public DoctorRepository(HealthCareSystemContext context)
        {
            _context = context;
        }
        public IEnumerable<Doctor> GetAll() => _context.Doctors.ToList();
        public Doctor GetById(int id) => _context.Doctors.Find(id);
        public void Add(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }
        public void Update(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                _context.SaveChanges();
            }
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

    }
}
