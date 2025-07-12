using HealthCareSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSystem.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("/Register")]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            
        }
    }
}
