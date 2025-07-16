using BusinessObjects;

namespace Repositories
{
    public interface IPatientRepository
    {
        Task CreatePatient(Patient patient);
    }
}
