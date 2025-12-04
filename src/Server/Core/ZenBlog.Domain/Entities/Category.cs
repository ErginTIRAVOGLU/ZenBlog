using System;
using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string CategoryName { get; set; } = default!;

    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
