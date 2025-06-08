using HealthCareSystem.Models;
using HealthCareSystem.Services;
using HealthCareSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSystem.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        public RegisterController(IUserService userService, IPatientService patientService, IDoctorService doctorService)
        {
            _userService = userService;
            _patientService = patientService;
            _doctorService = doctorService;
        }
        [HttpPost("/Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Partials/_RegisterPartial", model);
                }

                var authModel = new AuthViewModel
                {
                    Register = model,
                    ActiveForm = "register"
                };
                return View("~/Views/Login/Index.cshtml", authModel);
            }

            if (_userService.IsUserExists(model.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Partials/_RegisterPartial", model);
                }
                return View("~/Views/Login/Index.cshtml", new AuthViewModel { Register = model, ActiveForm = "register" });
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu không khớp");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("Partials/_RegisterPartial", model);
                }
                return View("~/Views/Login/Index.cshtml", new AuthViewModel { Register = model, ActiveForm = "register" });
            }

            var account = _userService.IsUserExists(model.Email);

            if (!account)
            {
                User user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = model.Password,
                    Role = model.Role,
                    CreatedAt = DateTime.Now,
                };
                _userService.CreateUser(user);

                if (user.Role.Equals("Patient"))
                {
                    Patient patient = new Patient
                    {
                        UserId = user.UserId
                    };
                    _patientService.CreatePatient(patient);
                }
                else if (user.Role.Equals("Doctor"))
                {
                    Doctor doctor = new Doctor
                    {
                        UserId = user.UserId
                    };
                    _doctorService.CreateDoctor(doctor);
                }
            }
            
            return RedirectToAction("Index", "Login");
        }
    }
}
