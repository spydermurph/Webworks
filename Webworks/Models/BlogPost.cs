using System;
using System.Collections.Generic;

namespace Webworks.Models;

public partial class BlogPost
{
    public long Id { get; set; }

    /// <summary>
    /// Slug for the post.
    /// </summary>
    public string Slug { get; set; } = null!;

    /// <summary>
    /// Date of publication of post
    /// </summary>
    public DateTime PublicationDate { get; set; }

    /// <summary>
    /// Post category id
    /// </summary>
    public long CategoryId { get; set; }

    public virtual ICollection<BlogPostContent> BlogPostContents { get; set; } = new List<BlogPostContent>();

    public virtual Category Category { get; set; } = null!;
}
