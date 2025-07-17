using BusinessObjects;

namespace Repositories.Interface
{
    public interface IDoctorRepository
    {
        public Task<Doctor> GetDoctorsByIdAsync(int id);
        Task<List<Doctor>> GetDoctorsAsync();
        Task<List<Doctor>> GetBySpecialtyAsync(int specialtyId);    }
}
