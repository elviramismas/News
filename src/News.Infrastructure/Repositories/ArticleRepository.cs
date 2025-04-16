using Microsoft.EntityFrameworkCore;
using News.Core.Entities;
using News.Core.Interfaces;
using News.Infrastructure.Database;

namespace News.Infrastructure.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly NewsDbContext _newsDbContext;
    
    public ArticleRepository(NewsDbContext newsDbContext)
    {
        _newsDbContext = newsDbContext;
    }
    
    public async Task<Article> Add(Article article)
    {
        article.CreatedAt = DateTime.UtcNow;
        
        _newsDbContext.Articles.Add(article);
        await _newsDbContext.SaveChangesAsync();
        
        return article;
    }

    public Task<Article?> GetBySlug(string slug)
    {
        return _newsDbContext.Articles.FirstOrDefaultAsync(a => a.Slug == slug);
    }

    public async Task<List<Article>> GetPaginated(int page, int pageSize)
    {
        return await _newsDbContext.Articles.Skip(page*pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<Article> Update(Article article, string oldSlug)
    {
        await _newsDbContext.Articles.Where(x => x.Slug == oldSlug)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Content, article.Content)
                .SetProperty(x => x.Slug, article.Slug)
                .SetProperty(x => x.Status, article.Status)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
            );
        
        return article;
    }

    public async Task Delete(string slug)
    {
        await _newsDbContext.Articles.Where(x => x.Slug == slug).ExecuteDeleteAsync();
    }
}