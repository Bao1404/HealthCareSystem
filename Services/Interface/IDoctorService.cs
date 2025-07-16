using BusinessObjects;

namespace Services.Interface
{
    public interface IDoctorService
    {
        public Task<Doctor> GetDoctorsByIdAsync(int id);
        Task<List<Doctor>> GetDoctorsAsync();
        Task<List<Doctor>> GetBySpecialtyAsync(int specialtyId);
        Task<Doctor?> GetByIdAsync(int doctorUserId);
    }
}
