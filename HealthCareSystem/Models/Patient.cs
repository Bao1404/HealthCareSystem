using System;
using System.Collections.Generic;

namespace HealthCareSystem.Models;

public partial class Patient
{
    public int UserId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? BloodType { get; set; }

    public string? Allergies { get; set; }

    public string? MedicalHistory { get; set; }

    public decimal? Bmi { get; set; }

    public string? Address { get; set; }

    public virtual User User { get; set; } = null!;
}
