using System;
using System.Collections.Generic;

namespace HealthCareSystem.Models;

public partial class Article
{
    public int ArticleId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? PublishedAt { get; set; }

    public string? ArticleImg { get; set; }
}
