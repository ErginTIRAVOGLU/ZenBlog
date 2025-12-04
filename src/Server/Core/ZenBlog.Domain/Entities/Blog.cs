using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Entities;

public class Blog : BaseEntity
{
    public string Title { get; set; } = default!;
    public string CoverImage { get; set; } = default!;
    public string BlogImage { get; set; } = default!;
    public string Description { get; set; } = default!;

    public Guid CategoryId { get; set; } 
    public virtual Category Category { get; set; }= null!;

    public string UserId { get; set; } = default!;
    public virtual AppUser User { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
