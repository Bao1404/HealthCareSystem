using BusinessObjects;

namespace Services
{
    public interface IPatientService
    {
        Task CreatePatient(Patient patient);
        Task<Patient?> GetByUserIdAsync(int userId);
        
        // Doctor-specific patient methods
        Task<List<Patient>> GetPatientsByDoctorAsync(int doctorId);
        Task<Patient?> GetPatientWithDetailsAsync(int userId);
        Task<List<Patient>> GetPatientsByDoctorAndStatusAsync(int doctorId, string status);
        Task<List<Patient>> SearchPatientsAsync(int doctorId, string searchTerm);
        Task<List<Patient>> GetCriticalPatientsAsync(int doctorId);
        Task<List<Patient>> GetFollowUpPatientsAsync(int doctorId);
        Task<List<Patient>> GetNewPatientsAsync(int doctorId, int daysThreshold = 30);
        Task<List<Patient>> GetActivePatientsAsync(int doctorId);
    }
}