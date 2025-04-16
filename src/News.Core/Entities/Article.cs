using System.Net;
using Microsoft.AspNetCore.Identity;
using News.Core.Enums;

namespace News.Core.Entities;

public class Article
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public ArticleStatus Status { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string AuthorId { get; set; }
    public ApplicationUser Author { get; set; }
}