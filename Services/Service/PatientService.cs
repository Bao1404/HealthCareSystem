using BusinessObjects;
using Repositories.Interface;

namespace Services.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public Task CreatePatient(Patient patient) => _patientRepository.CreatePatient(patient);
        public async Task<Patient?> GetByUserIdAsync(int userId) => await _patientRepository.GetByUserIdAsync(userId);
        public Task<List<Patient>> GetAllPatientsAsync() => _patientRepository.GetAllPatientsAsync();
    }
}