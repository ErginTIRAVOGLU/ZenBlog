using System;
using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Entities;

public sealed class Social : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string Icon { get; set; } = default!;
    
}
