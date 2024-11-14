using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class Tag : DBEntry
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Article>? Articles { get; set; }
}
