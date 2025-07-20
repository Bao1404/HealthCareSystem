using BusinessObjects;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAllPatients();
        Patient GetPatientById(int id);
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeletePatient(int id);
        Patient GetByUserId(int userId);
    }
}