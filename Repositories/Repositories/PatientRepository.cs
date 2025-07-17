using BusinessObjects;
using Repositories.Interface;

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

    }
}
