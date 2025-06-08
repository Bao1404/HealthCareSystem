namespace HealthCareSystem.ViewModels
{
    public class AuthViewModel
    {
        public LoginViewModel Login { get; set; } = new LoginViewModel();
        public RegisterViewModel Register { get; set; } = new RegisterViewModel();
        public string ActiveForm { get; set; } = "login"; // mặc định là login
    }
}
