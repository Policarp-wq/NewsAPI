using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class Tag : DBEntry
{
    public string Name { get; set; } = null!;
    [ValidateNever]
    public virtual ICollection<Article>? Articles { get; set; }
}
