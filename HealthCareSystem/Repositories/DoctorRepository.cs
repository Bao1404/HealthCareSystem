using HealthCareSystem.Models;
using HealthCareSystem.Services;

namespace HealthCareSystem.Repositories
{
    public class DoctorRepository : IDoctorService
    {
        private readonly HealthCareSystemContext _context;
        public DoctorRepository(HealthCareSystemContext context)
        {
            _context = context;
        }
        public void CreateDoctor(Doctor doctor)
        {
            if (doctor != null)
            {
                _context.Doctors.Add(doctor);
                _context.SaveChanges();
            }
        }
    }
}
