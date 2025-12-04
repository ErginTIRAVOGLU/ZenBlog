using System;
using ZenBlog.Domain.Dto.Common;
using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Dto;

public sealed class MessageDto:BaseEntityDto
{
    public string Name { get; set; } = default!;
    public string EMail { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string MessageBody { get; set; } = default!;
    public bool IsRead { get; set; } = false;
}
