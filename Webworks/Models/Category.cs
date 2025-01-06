using System;
using System.Collections.Generic;

namespace Webworks.Models;

public partial class Category
{
    /// <summary>
    /// Id of category
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// Category name of post
    /// </summary>
    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
