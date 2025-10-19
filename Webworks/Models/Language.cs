using System;
using System.Collections.Generic;

namespace Webworks.Models;

public partial class Language
{
    public long Id { get; set; }

    /// <summary>
    /// language + region,  e.g. &apos;en-US&apos;
    /// </summary>
    public string LocaleCode { get; set; } = null!;

    /// <summary>
    /// language, e.g. &apos;en&apos;
    /// </summary>
    public string LanguageCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    /// <summary>
    /// Direction of language, &apos;LTR&apos; or &apos;RTL&apos;
    /// </summary>
    public string Direction { get; set; } = null!;

    public virtual ICollection<BlogPostContent> BlogPostContents { get; set; } = new List<BlogPostContent>();
}
