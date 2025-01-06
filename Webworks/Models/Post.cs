using System;
using System.Collections.Generic;

namespace Webworks.Models;

public partial class Post
{
    public long PostId { get; set; }

    /// <summary>
    /// Title of the post.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Content of the post.
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// Post category id
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// Date of publication of post
    /// </summary>
    public DateTime PublicationDate { get; set; }

    public virtual Category Category { get; set; } = null!;
}
