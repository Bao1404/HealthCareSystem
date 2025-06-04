using System;
using System.Collections.Generic;

namespace HealthCareSystem.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int PatientUserId { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User PatientUser { get; set; } = null!;
}
