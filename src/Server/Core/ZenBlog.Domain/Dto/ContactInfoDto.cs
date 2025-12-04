using ZenBlog.Domain.Dto.Common;

namespace ZenBlog.Domain.Dto;

public sealed class ContactInfoDto : BaseEntityDto
{
    public string Address { get; set; } = default!;
    public string EMail { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string MapUrl { get; set; } = default!;
}
