using BusinessObjects;

namespace Repositories.Interface
{
    public interface IDoctorRepository
    {
        public Task<Doctor> GetDoctorsByIdAsync(int id);
    }
}
