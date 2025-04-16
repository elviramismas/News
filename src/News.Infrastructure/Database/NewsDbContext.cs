using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using News.Core.Entities;

namespace News.Infrastructure.Database;

public class NewsDbContext : IdentityDbContext<ApplicationUser>
{
    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) { }
    
    public DbSet<Article> Articles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Article>()
            .HasIndex(x => x.Slug)
            .IsUnique();

        builder.Entity<Article>()
            .HasOne(a => a.Author)
            .WithMany(u => u.Articles)
            .HasForeignKey(a => a.AuthorId);
        
        base.OnModelCreating(builder);
    }
    
}