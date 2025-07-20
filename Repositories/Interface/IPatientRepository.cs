using BusinessObjects;
using System.Collections.Generic;

namespace Repositories.Interface
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAll();
        Patient GetById(int id);
        void Add(Patient patient);
        void Update(Patient patient);
        void Delete(int id);
        Task CreatePatient(Patient patient);

        Task<Patient?> GetByUserIdAsync(int userId);

        Task<List<Patient>> GetAllPatientsAsync();
    }
}
