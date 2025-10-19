using System;
using System.Collections.Generic;

namespace Webworks.Models;

public partial class Category
{
    /// <summary>
    /// Id of category
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Category name
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
