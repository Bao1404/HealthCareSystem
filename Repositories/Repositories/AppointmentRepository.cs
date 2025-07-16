using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthCareSystemContext _context;

        public AppointmentRepository(HealthCareSystemContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.DoctorUser)
                    .ThenInclude(d => d.User)           // Include Doctor.User
                .Include(a => a.DoctorUser)
                    .ThenInclude(d => d.Specialty)      // Include Doctor.Specialty
                .Include(a => a.PatientUser)
                    .ThenInclude(p => p.User)           // Include Patient.User
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentsByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.DoctorUser)
                    .ThenInclude(d => d.User)
                .Include(a => a.DoctorUser)
                    .ThenInclude(d => d.Specialty)
                .Include(a => a.PatientUser)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        // Check for doctor's appointment times (used when making an appointment)
        public async Task<bool> IsTimeSlotBookedAsync(int doctorId, DateTime dateTime)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.DoctorUserId == doctorId &&
                a.AppointmentDateTime == dateTime &&
                a.Status != "Cancelled");
        }

        // Check for doctor's appointment times excluding a specific appointment (used when rescheduling)
        public async Task<bool> IsTimeSlotBookedAsync(int doctorId, DateTime dateTime, int excludeAppointmentId)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.DoctorUserId == doctorId &&
                a.AppointmentDateTime == dateTime &&
                a.Status != "Cancelled" &&
                a.AppointmentId != excludeAppointmentId);
        }

        // Get an appointment by doctor and specific date
        public async Task<List<Appointment>> GetByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            return await _context.Appointments.Where(a => a.DoctorUserId == doctorId
                                                    && a.AppointmentDateTime.Date == date.Date
                                                    && a.Status != "Cancelled")
                                              .ToListAsync();
        }
    }
}