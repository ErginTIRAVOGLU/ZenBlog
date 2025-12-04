using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Entities;

public class Category : BaseEntity
{
    public string CategoryName { get; set; } = default!;

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
