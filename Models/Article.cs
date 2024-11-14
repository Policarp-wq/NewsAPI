using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class Article
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public string? Header { get; set; }

    public string? Content { get; set; }

    public DateTime PostTime { get; set; }

    public User AuthorUser { get; set; } = null!;

    public ICollection<Comment>? Comments { get; set; }

    public ICollection<Tag>? Tags { get; set; }
}
