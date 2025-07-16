using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly HealthCareSystemContext _context;
        public SpecialtyRepository(HealthCareSystemContext context)
        {
            _context = context;
        }
        public async Task<List<Specialty>> GetAllSpecialtiesAsync()
        {
            return await _context.Specialties.ToListAsync();
        }
    }
}
