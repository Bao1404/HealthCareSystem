using HealthCareSystem.Services;
using HealthCareSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            var model = new AuthViewModel
            {
                Login = new LoginViewModel(),
                Register = new RegisterViewModel(),
                ActiveForm = "login"
            };
            return View(model);
        }
        [HttpPost("/Login")]
        public IActionResult Login(LoginViewModel model)
        {
            var auModel = new AuthViewModel
            {
                Login = new LoginViewModel(),
                Register = new RegisterViewModel(),
                ActiveForm = "login"
            };

            if (!ModelState.IsValid)
            {
                return View("Index", auModel);
            }

            var account = _userService.GetUserByAccount(model.Username, model.Password);
            if (account != null)
            {
                HttpContext.Session.SetObject("userId", account.UserId);
                HttpContext.Session.SetObject("userName", account.FullName);
                return RedirectToAction("Index", "Home");
                //if (account.RoleId == 1)
                //{
                //    return RedirectToAction("Admin", "Admin");
                //}
                //else
                //{
                //    return RedirectToAction("Index", "Home");
                //}
            }
            else
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
                auModel.Login = model;
                return View("Index", auModel);
            }
        }
    }
}
