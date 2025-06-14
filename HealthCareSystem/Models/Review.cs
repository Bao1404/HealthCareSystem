﻿using System;
using System.Collections.Generic;

namespace HealthCareSystem.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int PatientUserId { get; set; }

    public int DoctorUserId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User DoctorUser { get; set; } = null!;

    public virtual User PatientUser { get; set; } = null!;
}
