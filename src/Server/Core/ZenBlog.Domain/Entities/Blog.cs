using System;
using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Entities;

public sealed class Blog : BaseEntity
{
    public string Title { get; set; } = default!;
    public string CoverImage { get; set; } = default!;
    public string BlogImage { get; set; } = default!;
    public string Description { get; set; } = default!;

    public Guid CategoryId { get; set; } 
    public Category Category { get; set; }= null!;
}
