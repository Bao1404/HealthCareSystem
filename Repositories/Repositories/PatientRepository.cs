using BusinessObjects;
using Repositories.Interface;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Repositories.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthCareSystemContext _context;
        public PatientRepository(HealthCareSystemContext context)
        {
            _context = context;
        }
        public IEnumerable<Patient> GetAll() => _context.Patients.ToList();
        public Patient GetById(int id) => _context.Patients.Find(id);
        public void Add(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }
        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                _context.SaveChanges();
            }
        }
        public async Task CreatePatient(Patient patient)
        {
            try
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            return await _context.Patients.FindAsync(userId);
        }
        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            try
            {
                return await _context.Patients.Include(d => d.User).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
