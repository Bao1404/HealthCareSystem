using BusinessObjects;
using Repositories.Interface;
using Services.Interface;

namespace Services.Service
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
