using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Entities;

public class SubComment : BaseEntity
{

     public string UserId { get; set; } = default!;
    public virtual AppUser User { get; set; } = null!;
    public string Body { get; set; } = default!;
    public DateTime CommentDate { get; set; } = default!;

    public Guid CommentId { get; set; } = default!;
    public virtual Comment Comment { get; set; } = null!;

}