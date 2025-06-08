using System;
using System.Collections.Generic;

namespace HealthCareSystem.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public string? TableName { get; set; }

    public int? RecordId { get; set; }

    public string? Action { get; set; }

    public int? ChangedBy { get; set; }

    public DateTime? ChangedAt { get; set; }

    public string? Changes { get; set; }

    public virtual User? ChangedByNavigation { get; set; }
}
