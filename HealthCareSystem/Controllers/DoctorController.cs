using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using BusinessObjects;

namespace HealthCareSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public DoctorController(IDoctorService doctorService, IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Dashboard";
            return View();
        }
        public async Task<IActionResult> Appointments()
        {
            ViewData["ActiveMenu"] = "Appointments";
            
            // Get doctor ID from session (you should implement proper authentication)
            // For demo purposes, assuming doctor ID is stored in session
            var doctorId = HttpContext.Session.GetInt32("UserId") ?? 1; // Default to 1 for testing

            var pendingAppointments = await _appointmentService.GetPendingAppointmentsByDoctorAsync(doctorId);
            var todayAppointments = await _appointmentService.GetTodayAppointmentsByDoctorAsync(doctorId);
            var upcomingAppointments = await _appointmentService.GetUpcomingAppointmentsByDoctorAsync(doctorId);
            var completedAppointments = await _appointmentService.GetCompletedAppointmentsByDoctorAsync(doctorId);
            var cancelledAppointments = await _appointmentService.GetCancelledAppointmentsByDoctorAsync(doctorId);

            ViewBag.PendingAppointments = pendingAppointments;
            ViewBag.TodayAppointments = todayAppointments;
            ViewBag.UpcomingAppointments = upcomingAppointments;
            ViewBag.CompletedAppointments = completedAppointments;
            ViewBag.CancelledAppointments = cancelledAppointments;
            ViewBag.PendingCount = pendingAppointments.Count;

            return View();
        }
        public IActionResult Patients()
        {
            ViewData["ActiveMenu"] = "Patients";
            return View();
        }
        public IActionResult Schedule()
        {
            ViewData["ActiveMenu"] = "Schedule";
            return View();
        }
        public async Task<IActionResult> ProfileAsync(int id)
        {
            var doctor = await _doctorService.GetDoctorsByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }
        public IActionResult Calendar()
        {
            ViewData["ActiveMenu"] = "Calendar";
            return View();
        }
        public IActionResult Messages()
        {
            ViewData["ActiveMenu"] = "Messages";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveAppointment(int appointmentId)
        {
            var doctorId = HttpContext.Session.GetInt32("UserId") ?? 1;
            var result = await _appointmentService.ApproveAppointmentAsync(appointmentId, doctorId);
            
            if (result)
            {
                TempData["SuccessMessage"] = "Appointment approved successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to approve appointment.";
            }

            return RedirectToAction("Appointments");
        }

        [HttpPost]
        public async Task<IActionResult> RejectAppointment(int appointmentId, string? reason)
        {
            var doctorId = HttpContext.Session.GetInt32("UserId") ?? 1;
            var result = await _appointmentService.RejectAppointmentAsync(appointmentId, doctorId, reason);
            
            if (result)
            {
                TempData["SuccessMessage"] = "Appointment rejected successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to reject appointment.";
            }

            return RedirectToAction("Appointments");
        }

        public async Task<IActionResult> AppointmentDetails(int id)
        {
            var appointment = await _appointmentService.GetAppointmentsByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var doctorId = HttpContext.Session.GetInt32("UserId") ?? 1;
            if (appointment.DoctorUserId != doctorId)
            {
                return Forbid();
            }

            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteAppointment(int appointmentId)
        {
            try
            {
                var doctorId = HttpContext.Session.GetInt32("UserId") ?? 1;
                var appointment = await _appointmentService.GetAppointmentsByIdAsync(appointmentId);
                
                if (appointment == null)
                {
                    TempData["ErrorMessage"] = "Appointment not found.";
                    return RedirectToAction("Appointments");
                }

                // Check if the appointment belongs to the current doctor
                if (appointment.DoctorUserId != doctorId)
                {
                    TempData["ErrorMessage"] = "You are not authorized to complete this appointment.";
                    return RedirectToAction("Appointments");
                }

                // Check if appointment is in a valid status to be completed
                if (appointment.Status != "Confirmed")
                {
                    TempData["ErrorMessage"] = $"Cannot complete appointment with status '{appointment.Status}'. Only confirmed appointments can be completed.";
                    return RedirectToAction("Appointments");
                }

                // Update appointment status
                appointment.Status = "Completed";
                appointment.UpdatedAt = DateTime.Now;
                
                await _appointmentService.UpdateAppointmentAsync(appointment);
                
                TempData["SuccessMessage"] = "Appointment completed successfully!";
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging configured
                TempData["ErrorMessage"] = "An error occurred while completing the appointment.";
            }
            
            return RedirectToAction("Appointments");
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentInfo(int appointmentId)
        {
            try
            {
                var doctorId = HttpContext.Session.GetInt32("UserId") ?? 1;
                var appointment = await _appointmentService.GetAppointmentsByIdAsync(appointmentId);
                
                if (appointment == null || appointment.DoctorUserId != doctorId)
                {
                    return Json(new { success = false, message = "Appointment not found or unauthorized." });
                }

                return Json(new 
                { 
                    success = true, 
                    patientName = appointment.PatientUser.User.FullName,
                    appointmentDate = appointment.AppointmentDateTime.ToString("MMM dd, yyyy - HH:mm"),
                    notes = appointment.Notes ?? "No additional notes"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error retrieving appointment information." });
            }
        }
    }
}