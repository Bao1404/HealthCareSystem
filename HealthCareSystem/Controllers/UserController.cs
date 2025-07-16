using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace HealthCareSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "Dashboard";
            var currentUser = HttpContext.Session.GetInt32("UserId");
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var user = await _userService.GetUserById(currentUser.Value);
            return View("Index", user);
        }
        public IActionResult Appointments()
        {
            ViewData["ActiveMenu"] = "Appointments";
            return View();
        }
        public IActionResult Calendar()
        {
            ViewData["ActiveMenu"] = "Calendar";
            return View();
        }
        public IActionResult Doctors()
        {
            ViewData["ActiveMenu"] = "Doctors";
            return View();
        }
        public IActionResult Messages()
        {
            ViewData["ActiveMenu"] = "Messages";
            return View();
        }
        public IActionResult ChatBox()
        {
            ViewData["ActiveMenu"] = "ChatBox";
            return View();
        }
        public IActionResult Profile()
        {
            ViewData["ActiveMenu"] = "Profile";
            return View();
        }
    }
}
