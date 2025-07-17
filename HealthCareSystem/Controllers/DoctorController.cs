using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using BusinessObjects;
using HealthCareSystem.Models;

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
        public async Task<IActionResult> Calendar(int? year, int? month)
        {
            ViewData["ActiveMenu"] = "Calendar";
            
            var doctorId = HttpContext.Session.GetInt32("UserId") ?? 1;
            var currentDate = year.HasValue && month.HasValue 
                ? new DateTime(year.Value, month.Value, 1) 
                : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var calendarViewModel = await BuildCalendarViewModelAsync(doctorId, currentDate);
            
            return View(calendarViewModel);
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

        private async Task<CalendarViewModel> BuildCalendarViewModelAsync(int doctorId, DateTime currentMonth)
        {
            // Get appointments for the month
            var monthAppointments = await _appointmentService.GetAppointmentsByMonthAsync(doctorId, currentMonth);
            
            // Get today's appointments
            var todayAppointments = await _appointmentService.GetTodayAppointmentsByDoctorAsync(doctorId);
            
            // Get upcoming appointments (next 7 days)
            var upcomingAppointments = await _appointmentService.GetUpcomingAppointmentsByDoctorAsync(doctorId);
            var nextWeekAppointments = upcomingAppointments.Where(a => a.AppointmentDateTime <= DateTime.Now.AddDays(7)).ToList();

            var calendarViewModel = new CalendarViewModel
            {
                CurrentMonth = currentMonth,
                DoctorId = doctorId,
                TodayAppointments = MapToCalendarItems(todayAppointments),
                UpcomingAppointments = MapToCalendarItems(nextWeekAppointments),
                CalendarDays = BuildCalendarDays(currentMonth, monthAppointments)
            };

            return calendarViewModel;
        }

        private List<CalendarDay> BuildCalendarDays(DateTime currentMonth, List<Appointment> monthAppointments)
        {
            var calendarDays = new List<CalendarDay>();
            
            // Get the first day of the month and find the start of the calendar grid
            var firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var startDate = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
            
            // Build 42 days (6 weeks) for the calendar grid
            for (int i = 0; i < 42; i++)
            {
                var currentDate = startDate.AddDays(i);
                var dayAppointments = monthAppointments
                    .Where(a => a.AppointmentDateTime.Date == currentDate.Date)
                    .ToList();

                calendarDays.Add(new CalendarDay
                {
                    Date = currentDate,
                    IsCurrentMonth = currentDate.Month == currentMonth.Month,
                    IsToday = currentDate.Date == DateTime.Today,
                    Appointments = MapToCalendarItems(dayAppointments),
                    AppointmentCount = dayAppointments.Count
                });
            }

            return calendarDays;
        }

        private List<AppointmentCalendarItem> MapToCalendarItems(List<Appointment> appointments)
        {
            return appointments.Select(a => new AppointmentCalendarItem
            {
                AppointmentId = a.AppointmentId,
                PatientName = a.PatientUser?.User?.FullName ?? "Unknown Patient",
                AppointmentDateTime = a.AppointmentDateTime,
                Status = a.Status ?? "Unknown",
                Notes = a.Notes ?? "",
                AppointmentType = GetAppointmentType(a.Notes),
                StatusColor = GetStatusColor(a.Status),
                TimeDisplay = a.AppointmentDateTime.ToString("HH:mm"),
                DateDisplay = a.AppointmentDateTime.ToString("MMM dd")
            }).ToList();
        }

        private string GetAppointmentType(string? notes)
        {
            if (string.IsNullOrEmpty(notes)) return "General";
            
            var lowerNotes = notes.ToLower();
            if (lowerNotes.Contains("follow-up")) return "Follow-up";
            if (lowerNotes.Contains("check-up")) return "Check-up";
            if (lowerNotes.Contains("emergency")) return "Emergency";
            if (lowerNotes.Contains("consultation")) return "Consultation";
            
            return "General";
        }

        private string GetStatusColor(string? status)
        {
            return status?.ToLower() switch
            {
                "pending" => "warning",
                "confirmed" => "primary",
                "completed" => "success",
                "cancelled" => "danger",
                _ => "secondary"
            };
        }
    }
}