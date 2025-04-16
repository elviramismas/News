using News.Core.Entities;
using News.Core.Enums;

namespace News.Core.Models;

public class ArticleModel
{
    public string Content { get; set; }
    public string Title { get; set; }
    public Guid AuthorId { get; set; }
    public string Slug { get; set; }
    public ArticleStatus Status { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public static ArticleModel FromEntity(Article entity)
    {
        return new ArticleModel
        {
            Slug = entity.Slug,
            Content = entity.Content,
            Title = entity.Title,
            Status = entity.Status,
            PublishedAt = entity.PublishedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
    
    public Article ToEntity()
    {
        return new Article
        {
            Slug = Slug,
            Content = Content,
            Title = Title,
            Status = Status,
            PublishedAt = PublishedAt,
            UpdatedAt = UpdatedAt
        };
    }
}