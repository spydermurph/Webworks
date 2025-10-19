using System;
using System.Collections.Generic;

namespace Webworks.Models;

public partial class BlogPostContent
{
    public long Id { get; set; }

    public long BlogPostId { get; set; }

    public long LanguageId { get; set; }

    public string Title { get; set; } = null!;

    public string Excerpt { get; set; } = null!;

    public string Content { get; set; } = null!;

    public virtual BlogPost BlogPost { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;
}
