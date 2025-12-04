using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Entities;

public  class ContactInfo : BaseEntity
{
    public string Address { get; set; } = default!;
    public string EMail { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string MapUrl { get; set; } = default!;
}
