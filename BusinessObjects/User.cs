using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class User
{
    public int Id { get; set; }

    public string? Fullname { get; set; }

    public string Mail { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
