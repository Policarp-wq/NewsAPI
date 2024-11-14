using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class Article : DBEntry
{
    public int AuthorId { get; set; }

    public string? Header { get; set; }

    public string? Content { get; set; }

    public DateTime PostTime { get; set; }
    [ValidateNever]
    public User? AuthorUser { get; set; }
    [ValidateNever]
    public ICollection<Comment>? Comments { get; set; }
    [ValidateNever]
    public ICollection<Tag>? Tags { get; set; }
}
