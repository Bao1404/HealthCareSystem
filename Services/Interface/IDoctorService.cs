using BusinessObjects;

namespace Services.Interface
{
    public interface IDoctorService
    {
        Task<Doctor> GetDoctorsByIdAsync(int id);
        Task<List<Doctor>> GetDoctorsAsync();
        Task<List<Doctor>> GetBySpecialtyAsync(int specialtyId);
        Task UpdateImageUrlDoctor(string url, int userId);
    }
}
