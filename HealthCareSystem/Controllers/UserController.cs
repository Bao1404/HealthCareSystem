using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Threading.Tasks;

namespace HealthCareSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private int? currentUser => HttpContext.Session.GetInt32("UserId");
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "Dashboard";
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var user = await _userService.GetUserById(currentUser.Value);
            return View(user);
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
        public async Task<IActionResult> ChatBox()
        {
            if(currentUser == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var user = await _userService.GetUserById(currentUser.Value);
            ViewData["ActiveMenu"] = "ChatBox";
            return View(user);
        }
        public IActionResult Profile()
        {
            ViewData["ActiveMenu"] = "Profile";
            return View();
        }
    }
}
