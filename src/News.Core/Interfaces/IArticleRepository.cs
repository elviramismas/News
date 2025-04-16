using News.Core.Entities;

namespace News.Core.Interfaces;

public interface IArticleRepository
{
    Task<Article> Add(Article article);
    Task<Article?> GetBySlug(string slug);
    Task<List<Article>> GetPaginated(int page, int pageSize);
    Task<Article> Update(Article article, string slug);
    Task Delete(string slug);
}