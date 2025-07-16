using BusinessObjects;

namespace Services
{
    public interface IPatientService
    {
        Task CreatePatient(Patient patient);
    }
}
