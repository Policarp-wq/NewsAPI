using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class User : DBEntry
{
    public string Fullname { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Email { get; set; }

    public ICollection<Article>? Articles { get; set; }

    public ICollection<Comment>? Comments { get; set; }
}
