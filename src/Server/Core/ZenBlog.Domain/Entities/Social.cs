using ZenBlog.Domain.Entities.Common;

namespace ZenBlog.Domain.Entities;

public  class Social : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string Icon { get; set; } = default!;
    
}
