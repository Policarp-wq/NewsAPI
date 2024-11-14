using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class Comment : DBEntry
{
    public int AuthorId { get; set; }

    public int ArticleId { get; set; }

    public string? Content { get; set; }

    public DateTime PostTime { get; set; }
    [ValidateNever]
    public Article Article { get; set; } = null!;
    [ValidateNever]
    public User AuthorUser { get; set; } = null!;

}
