using BusinessObjects;
using Repositories.Interface;
using Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Services.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public IEnumerable<Patient> GetAllPatients() => _patientRepository.GetAll();
        public Patient GetPatientById(int id) => _patientRepository.GetById(id);
        public Patient GetByUserId(int userId) => _patientRepository.GetAll().FirstOrDefault(p => p.UserId == userId);
        public void AddPatient(Patient patient) => _patientRepository.Add(patient);
        public void UpdatePatient(Patient patient) => _patientRepository.Update(patient);
        public void DeletePatient(int id) => _patientRepository.Delete(id);
    }
}