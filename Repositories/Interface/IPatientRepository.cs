using BusinessObjects;

namespace Repositories.Interface
{
    public interface IPatientRepository
    {
        Task CreatePatient(Patient patient);

        Task<Patient?> GetByUserIdAsync(int userId);

    }
}
