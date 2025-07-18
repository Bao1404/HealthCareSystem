using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCareSystem.ViewModels
{
    public class PatientViewModel
    {
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        public string? Password { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? BloodType { get; set; }

        public string? Allergies { get; set; }

        [Range(0, 500)]
        public int? Weight { get; set; }

        [Range(0, 300)]
        public int? Height { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Emergency phone number must be exactly 10 digits.")]
        [Phone]
        public string? EmergencyPhoneNumber { get; set; }

        public decimal? Bmi { get; set; }
    }
}