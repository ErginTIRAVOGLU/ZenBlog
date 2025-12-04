using System;
using ZenBlog.Domain.Dto.Common;

namespace ZenBlog.Domain.Dto;

public sealed class SocialDto : BaseEntityDto
{
    public string Title { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string Icon { get; set; } = default!;
}
