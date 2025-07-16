using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentsByIdAsync(int id);
        Task AddAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(int id);
        Task<bool> IsTimeSlotBookedAsync(int doctorId, DateTime dateTime);
        Task<bool> IsTimeSlotBookedAsync(int doctorId, DateTime dateTime, int excludeAppointmentId);
        Task<List<Appointment>> GetByDoctorAndDateAsync(int doctorId, DateTime date);
    }
}
