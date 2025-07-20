using BusinessObjects;
using Repositories.Interface;
using Services;

namespace Services.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public async Task CreatePatient(Patient patient)
        {
            await _patientRepository.CreatePatient(patient);
        }
        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            return await _patientRepository.GetByUserIdAsync(userId);
        }

        // Doctor-specific patient methods
        public async Task<List<Patient>> GetPatientsByDoctorAsync(int doctorId)
        {
            return await _patientRepository.GetPatientsByDoctorAsync(doctorId);
        }

        public async Task<Patient?> GetPatientWithDetailsAsync(int userId)
        {
            return await _patientRepository.GetPatientWithDetailsAsync(userId);
        }

        public async Task<List<Patient>> GetPatientsByDoctorAndStatusAsync(int doctorId, string status)
        {
            return await _patientRepository.GetPatientsByDoctorAndStatusAsync(doctorId, status);
        }

        public async Task<List<Patient>> SearchPatientsAsync(int doctorId, string searchTerm)
        {
            return await _patientRepository.SearchPatientsAsync(doctorId, searchTerm);
        }

        public async Task<List<Patient>> GetCriticalPatientsAsync(int doctorId)
        {
            return await _patientRepository.GetCriticalPatientsAsync(doctorId);
        }

        public async Task<List<Patient>> GetFollowUpPatientsAsync(int doctorId)
        {
            return await _patientRepository.GetFollowUpPatientsAsync(doctorId);
        }

        public async Task<List<Patient>> GetNewPatientsAsync(int doctorId, int daysThreshold = 30)
        {
            return await _patientRepository.GetNewPatientsAsync(doctorId, daysThreshold);
        }

        public async Task<List<Patient>> GetActivePatientsAsync(int doctorId)
        {
            return await _patientRepository.GetActivePatientsAsync(doctorId);
        }
    }
}