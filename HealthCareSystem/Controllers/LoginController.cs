using HealthCareSystem.Services;
using HealthCareSystem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                auModel.Login = model;
                return View("Index", auModel);
            }

            var account = _userService.GetUserByAccount(model.Username, model.Password);
            if (account != null)
            {
                // Tạo claim cho người dùng
                var claims = new[]
                {
            new Claim(ClaimTypes.Name, account.FullName),
            new Claim(ClaimTypes.NameIdentifier, account.UserId.ToString()),
            // Thêm claim cho role nếu cần
             new Claim(ClaimTypes.Role, account.Role)
        };

                var identity = new ClaimsIdentity(claims, "Login");
                var principal = new ClaimsPrincipal(identity);

                // Đăng nhập
                HttpContext.SignInAsync(principal).Wait();

                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetObject("userId", account.UserId);
                HttpContext.Session.SetObject("userName", account.FullName);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
                auModel.Login = model;
                return View("Index", auModel);
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
