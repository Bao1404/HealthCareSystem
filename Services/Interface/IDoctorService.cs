using BusinessObjects;

namespace Services.Interface
{
    public interface IDoctorService
    {
        public Task<Doctor> GetDoctorsByIdAsync(int id);
    }
}
