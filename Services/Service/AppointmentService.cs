using BusinessObjects;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public async Task<List<Appointment>> GetAllAppointmentsAsync() => await _appointmentRepository.GetAllAppointmentsAsync();
        public async Task<Appointment?> GetAppointmentsByIdAsync(int id) => await _appointmentRepository.GetAppointmentsByIdAsync(id);
        public async Task AddAppointmentAsync(Appointment appointment) => await _appointmentRepository.AddAppointmentAsync(appointment);
        public async Task UpdateAppointmentAsync(Appointment appointment) => await _appointmentRepository.UpdateAppointmentAsync(appointment);
        public async Task DeleteAppointmentAsync(int id) => await _appointmentRepository.DeleteAppointmentAsync(id);
        public async Task<bool> IsTimeSlotBookedAsync(int doctorId, DateTime dateTime) => 
            await _appointmentRepository.IsTimeSlotBookedAsync(doctorId, dateTime);
        public async Task<bool> IsTimeSlotBookedAsync(int doctorId, DateTime dateTime, int excludeAppointmentId) =>
            await _appointmentRepository.IsTimeSlotBookedAsync(doctorId, dateTime, excludeAppointmentId);
        public async Task<List<Appointment>> GetByDoctorAndDateAsync(int doctorId, DateTime date) =>
            await _appointmentRepository.GetByDoctorAndDateAsync(doctorId, date);
    }
}
