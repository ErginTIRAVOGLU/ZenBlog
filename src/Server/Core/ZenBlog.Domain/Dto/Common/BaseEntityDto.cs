using System;

namespace ZenBlog.Domain.Dto.Common;

public class BaseEntityDto
{
    public Guid Id { get; set; } 
    public DateTime CreatedAt { get; set; }  
    public DateTime? UpdatedAt { get; set; }
}
