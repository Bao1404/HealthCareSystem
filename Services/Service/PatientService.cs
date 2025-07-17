using BusinessObjects;
using Repositories;

namespace Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public Task CreatePatient(Patient patient) => _patientRepository.CreatePatient(patient);
    }
}
