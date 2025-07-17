using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace HealthCareSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Dashboard";
            return View();
        }
        public IActionResult Appointments()
        {
            ViewData["ActiveMenu"] = "Appointments";
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
            var doctorId = HttpContext.Session.GetInt32("UserId");

            if (doctorId == null)
            {
                return RedirectToAction("Index", "Login"); // hoặc thông báo lỗi
            }

            ViewData["DoctorId"] = doctorId; // nếu muốn gửi ra View
            ViewData["ActiveMenu"] = "Messages";
            return View();
        }

    }
}
