using Microsoft.AspNetCore.Mvc;

namespace HealthCareSystem.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
