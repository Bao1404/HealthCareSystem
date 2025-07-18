using BusinessObjects;

namespace Services
{
    public interface IPatientService
    {
        Task CreatePatient(Patient patient);
        Task<Patient?> GetByUserIdAsync(int userId);
        Task<List<Patient>> GetAllPatientsAsync();
    }
}