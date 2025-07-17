using BusinessObjects;

namespace Repositories.Interface
{
    public interface IDoctorRepository
    {
        public Task<Doctor> GetDoctorsByIdAsync(int id);
        Task<List<Doctor>> SearchDoctorsAsync(string fullName, string phoneNumber, string email, string specialtyName);
    }
}
