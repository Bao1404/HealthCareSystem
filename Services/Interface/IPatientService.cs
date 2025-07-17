using BusinessObjects;

namespace Services.Interface
{
    public interface IPatientService
    {
        Task CreatePatient(Patient patient);
    }
}
