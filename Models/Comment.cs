using System;
using System.Collections.Generic;

namespace NewsAPI.Models;

public partial class Comment : DBEntry
{
    public int AuthorId { get; set; }

    public int ArticleId { get; set; }

    public string? Content { get; set; }

    public DateTime PostTime { get; set; }

    public Article Article { get; set; } = null!;

    public User AuthorUser { get; set; } = null!;

}
