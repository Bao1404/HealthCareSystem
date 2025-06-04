using System;
using System.Collections.Generic;

namespace HealthCareSystem.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientUserId { get; set; }

    public int DoctorUserId { get; set; }

    public DateTime AppointmentDateTime { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Notes { get; set; }

    public virtual User DoctorUser { get; set; } = null!;

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual User PatientUser { get; set; } = null!;
}
