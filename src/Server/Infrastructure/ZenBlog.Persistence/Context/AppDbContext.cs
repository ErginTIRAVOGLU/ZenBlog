using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Persistence.Context;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, AppRole, string>(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<ContactInfo> ContactInfos { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Social> Socials { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<SubComment> SubComments { get; set; }


    override protected void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Comment>()
            .HasOne(c => c.Blog)
            .WithMany(b => b.Comments)
            .HasForeignKey(c => c.BlogId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<SubComment>()
            .HasOne(s => s.Comment)
            .WithMany(c => c.SubComments)
            .HasForeignKey(s => s.CommentId)
            .OnDelete(DeleteBehavior.NoAction);
            
        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Entity<SubComment>()
            .HasOne(s => s.User)
            .WithMany(u => u.SubComments)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
