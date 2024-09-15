using System;
using System.Collections.Generic;

namespace BlogPostProject.Models;

public partial class BlogPost
{
    public int BlogPostId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? PublishedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string AuthorName { get; set; } = null!;
}
