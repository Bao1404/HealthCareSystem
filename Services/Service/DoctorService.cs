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
        public async Task<List<Doctor>> GetDoctorsAsync() => 
            await _doctorRepository.GetDoctorsAsync();
        public async Task<List<Doctor>> GetBySpecialtyAsync(int specialtyId) => 
            await _doctorRepository.GetBySpecialtyAsync(specialtyId);
        public async Task UpdateImageUrlDoctor(string url, int userId)
        {
             await _doctorRepository.UpdateImageUrlDoctor(url, userId);
        }
    }
}
