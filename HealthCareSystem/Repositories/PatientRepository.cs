using HealthCareSystem.Models;
using HealthCareSystem.Services;

namespace HealthCareSystem.Repositories
{
    public class PatientRepository : IPatientService
    {
        private readonly HealthCareSystemContext _context;
        public PatientRepository(HealthCareSystemContext context)
        {
            _context = context;
        }
        public void CreatePatient(Patient patient)
        {
            if(patient != null)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
            }
        }
    }
}
