using ZenBlog.Domain.Dto.Common;

namespace ZenBlog.Domain.Dto;

public sealed class CommentDto : BaseEntityDto
{
    public string Body { get; set; } = default!;
    public DateTime CommentDate { get; set; } = default!;

    public string UserId { get; set; } = default!;

    public Guid BlogId { get; set; }
}
