using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        public PatientService()
        {
            _patientRepository = new PatientRepository();
        }
        public Task CreatePatient(Patient patient) => _patientRepository.CreatePatient(patient);
    }
}
