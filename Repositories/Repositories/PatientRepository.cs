using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthCareSystemContext _context;
        public PatientRepository(HealthCareSystemContext context)
        {
            _context = context;
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
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        // Doctor-specific patient methods
        public async Task<List<Patient>> GetPatientsByDoctorAsync(int doctorId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.DoctorUser)
                .Where(p => p.Appointments.Any(a => a.DoctorUserId == doctorId))
                .Distinct()
                .OrderBy(p => p.User.FullName)
                .ToListAsync();
        }

        public async Task<Patient?> GetPatientWithDetailsAsync(int userId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.DoctorUser)
                        .ThenInclude(d => d.User)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.MedicalRecords)
                .Include(p => p.MedicalHistories)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<List<Patient>> GetPatientsByDoctorAndStatusAsync(int doctorId, string status)
        {
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.DoctorUser)
                .Where(p => p.Appointments.Any(a => a.DoctorUserId == doctorId && a.Status == status))
                .Distinct()
                .OrderBy(p => p.User.FullName)
                .ToListAsync();
        }

        public async Task<List<Patient>> SearchPatientsAsync(int doctorId, string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.DoctorUser)
                .Where(p => p.Appointments.Any(a => a.DoctorUserId == doctorId) &&
                           (p.User.FullName.ToLower().Contains(lowerSearchTerm) ||
                            p.User.Email.ToLower().Contains(lowerSearchTerm) ||
                            (p.User.PhoneNumber != null && p.User.PhoneNumber.Contains(searchTerm))))
                .Distinct()
                .OrderBy(p => p.User.FullName)
                .ToListAsync();
        }

        public async Task<List<Patient>> GetCriticalPatientsAsync(int doctorId)
        {
            // Define critical patients as those with recent emergency appointments or specific conditions
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.DoctorUser)
                .Where(p => p.Appointments.Any(a => a.DoctorUserId == doctorId &&
                           (a.Notes != null && a.Notes.ToLower().Contains("emergency")) ||
                           (a.Notes != null && a.Notes.ToLower().Contains("urgent")) ||
                           (a.Notes != null && a.Notes.ToLower().Contains("critical"))))
                .Distinct()
                .OrderBy(p => p.User.FullName)
                .ToListAsync();
        }

        public async Task<List<Patient>> GetFollowUpPatientsAsync(int doctorId)
        {
            // Patients who need follow-up (completed appointments in last 30 days that mentioned follow-up)
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.DoctorUser)
                .Where(p => p.Appointments.Any(a => a.DoctorUserId == doctorId &&
                           a.Status == "Completed" &&
                           a.AppointmentDateTime >= thirtyDaysAgo &&
                           (a.Notes != null && a.Notes.ToLower().Contains("follow-up"))))
                .Distinct()
                .OrderBy(p => p.User.FullName)
                .ToListAsync();
        }

        public async Task<List<Patient>> GetNewPatientsAsync(int doctorId, int daysThreshold = 30)
        {
            var cutoffDate = DateTime.Now.AddDays(-daysThreshold);
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.DoctorUser)
                .Where(p => p.Appointments.Any(a => a.DoctorUserId == doctorId) &&
                           p.CreatedAt >= cutoffDate)
                .Distinct()
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Patient>> GetActivePatientsAsync(int doctorId)
        {
            // Patients with confirmed or completed appointments in the last 90 days
            var ninetyDaysAgo = DateTime.Now.AddDays(-90);
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.DoctorUser)
                .Where(p => p.Appointments.Any(a => a.DoctorUserId == doctorId &&
                           (a.Status == "Confirmed" || a.Status == "Completed") &&
                           a.AppointmentDateTime >= ninetyDaysAgo))
                .Distinct()
                .OrderBy(p => p.User.FullName)
                .ToListAsync();
        }
    }
}
