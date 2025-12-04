using Microsoft.AspNetCore.Identity;

namespace ZenBlog.Domain.Entities;

public class AppUser : IdentityUser<string>
{
    public AppUser() 
    {
        Id = Guid.CreateVersion7().ToString();
    }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? ImageUrl { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<SubComment> SubComments { get; set; } = new List<SubComment>();
}
