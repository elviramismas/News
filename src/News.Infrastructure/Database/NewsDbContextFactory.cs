using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace News.Infrastructure.Database;

public class NewsDbContextFactory : IDesignTimeDbContextFactory<NewsDbContext>
{
    NewsDbContext IDesignTimeDbContextFactory<NewsDbContext>.CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<NewsDbContext>();
        builder.UseNpgsql("Host=localhost; Port=5433; Database=News;Username=postgres;Password=EndavaNews123");

        return new NewsDbContext(builder.Options);
    }
}