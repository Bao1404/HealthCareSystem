using BusinessObjects;
using Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthCareSystemContext _context;
        public PatientRepository(HealthCareSystemContext context)
        {
            _context = context;
        }
        public async Task CreatePatient(Patient patient)
        {
            try
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            return await _context.Patients.FindAsync(userId);
        }
        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            try
            {
                return await _context.Patients.Include(d => d.User).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
