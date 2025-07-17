using BusinessObjects;

namespace Repositories.Interface
{
    public interface IPatientRepository
    {
        Task CreatePatient(Patient patient);
    }
}
