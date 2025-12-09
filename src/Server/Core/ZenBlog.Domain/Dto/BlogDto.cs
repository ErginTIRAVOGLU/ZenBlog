using ZenBlog.Domain.Dto.Common;

namespace ZenBlog.Domain.Dto;

public sealed class BlogDto:BaseEntityDto
{
    public string Title { get; set; } = default!;
    public string CoverImage { get; set; } = default!;
    public string BlogImage { get; set; } = default!;
    public string Description { get; set; } = default!;
   
    public Guid CategoryId { get; set; } 
    public string CategoryName { get; set; } = default!; 
    public string UserId { get; set; } = default!;
    public string UserName { get; set; } = default!; 
    
}
