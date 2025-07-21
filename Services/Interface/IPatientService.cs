using BusinessObjects;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAllPatients();
        Patient GetPatientById(int id);
        void AddPatient(Patient patient);
        Task UpdatePatient(Patient patient);
        void DeletePatient(int id);
        Patient GetByUserId(int userId);
        Task CreatePatient(Patient patient);
        Task<Patient?> GetByUserIdAsync(int userId);
        Task<List<Patient>> GetAllPatientsAsync();
    }
}