using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Doctor
{
    public int UserId { get; set; }

    public int? SpecialtyId { get; set; }

    public string? Qualifications { get; set; }

    public string? Experience { get; set; }

    public string? Bio { get; set; }

    public decimal? Rating { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Specialty? Specialty { get; set; }

    public virtual User User { get; set; } = null!;
}
