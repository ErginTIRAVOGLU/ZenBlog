using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Entities;

public class Comment : BaseEntity
{
    
    public string Body { get; set; } = default!;
    public DateTime CommentDate { get; set; } = default!;

    public string UserId { get; set; } = default!;
    public virtual AppUser User { get; set; } = null!;

    public Guid BlogId { get; set; } = default!;
    public virtual Blog Blog { get; set; } = null!;

    public virtual ICollection<SubComment> SubComments { get; set; } = new List<SubComment>();
}
