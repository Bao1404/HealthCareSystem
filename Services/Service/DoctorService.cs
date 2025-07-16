using BusinessObjects;
using Repositories.Interface;
using Services.Interface;

namespace Services.Service
{
    public class DoctorService : IDoctorService
    {
        private IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public Task<Doctor> GetDoctorsByIdAsync(int id)
        {
            return _doctorRepository.GetDoctorsByIdAsync(id);
        }
    }
}
