using System.Security;
using Microsoft.AspNetCore.Identity;
using News.Core.Entities;
using News.Core.Helpers;
using News.Core.Interfaces;
using News.Core.Models;

namespace News.Business.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public ArticleService(IArticleRepository articleRepository, UserManager<ApplicationUser> userManager)
    {
        _articleRepository = articleRepository;
        _userManager = userManager;
    }
    
    public async Task<List<ArticleModel>> GetPaginated(int page, int pageSize)
    {
        return (await _articleRepository.GetPaginated(page, pageSize))
            .Select(x => ArticleModel.FromEntity(x))
            .ToList();
    }

    public async Task<ArticleModel> Add(ArticleModel inputArticle, string userEmail)
    {
        inputArticle.Slug = SlugGenerator.GenerateSlug(inputArticle.Title);
        
        var articleEntity = inputArticle.ToEntity();
        
        var user = await _userManager.FindByEmailAsync(userEmail);
        
        articleEntity.AuthorId = user.Id;
        
        var article = await _articleRepository.Add(articleEntity);
        
        return ArticleModel.FromEntity(article);
    }

    public async Task<ArticleModel?> GetBySlug(string slug)
    {
        var article = await _articleRepository.GetBySlug(slug);

        if (article is null)
            return null;
        
        return ArticleModel.FromEntity(article);
    }

    public async Task<ArticleModel> Update(ArticleModel inputArticle, string userEmail)
    {
        var oldSlug = inputArticle.Slug;
        
        var article = await _articleRepository.GetBySlug(oldSlug);
        
        inputArticle.Slug = SlugGenerator.GenerateSlug(inputArticle.Title);
        
        var articleEntity = inputArticle.ToEntity();
        
        var user = await _userManager.FindByEmailAsync(userEmail);
        
        if(article.AuthorId != user.Id) 
            throw new SecurityException($"User {user.UserName} does not have an access to this article");
        
        var updatedArticle = await _articleRepository.Update(articleEntity, oldSlug);
        
        return ArticleModel.FromEntity(updatedArticle);
    }

    public async Task Delete(string slug)
    {
        await _articleRepository.Delete(slug);
    }
}