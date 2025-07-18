using System.ComponentModel.DataAnnotations;

namespace HealthCareSystem.ViewModels
{
    public class DoctorViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Specialty is required.")]
        public int SpecialtyId { get; set; }

        public string? Qualifications { get; set; }

        public string? Experience { get; set; }

        public string? Bio { get; set; }

        public decimal? Rating { get; set; }
    }
}