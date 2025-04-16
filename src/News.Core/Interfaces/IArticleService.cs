using News.Core.Models;

namespace News.Core.Interfaces;

public interface IArticleService
{
    Task<List<ArticleModel>> GetPaginated(int page, int pageSize);
    Task<ArticleModel> Add(ArticleModel article, string userEmail);
    Task<ArticleModel?> GetBySlug(string slug);
    Task<ArticleModel> Update(ArticleModel article, string userEmail);
    Task Delete(string slug);
}