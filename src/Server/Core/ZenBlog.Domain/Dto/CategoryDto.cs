using ZenBlog.Domain.Dto.Common;

namespace ZenBlog.Domain.Dto;

public sealed class CategoryDto : BaseEntityDto
{
     public string CategoryName { get; set; } = default!;

    public ICollection<BlogDto> Blogs { get; set; } = new List<BlogDto>();
}
