using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News.Core.Interfaces;
using News.Core.Models;

namespace News.API.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }
    
    [HttpGet]
    [Authorize(Roles = "Reader, Author")]
    public async Task<IActionResult> GetPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        return Ok(await _articleService.GetPaginated(page, pageSize));
    }
    
    [HttpGet("/{slug}")]
    [Authorize(Roles = "Reader, Author")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        if(string.IsNullOrEmpty(slug))
            return BadRequest();
        
        var article = await _articleService.GetBySlug(slug);
        
        if(article is null)
            return NotFound();

        return Ok(article);
    }
    
    [HttpPost]
    [Authorize(Roles = "Author")]
    public async Task<IActionResult> Add([FromBody] ArticleModel articleModel)
    {
        var userEmail = User.FindFirstValue(ClaimValueTypes.Email);
        
        return Ok(await _articleService.Add(articleModel, userEmail));
    }
    
    [HttpPut]
    [Authorize(Roles = "Author")]
    public async Task<IActionResult> Update([FromBody] ArticleModel articleModel)
    {
        var userEmail = User.FindFirstValue(ClaimValueTypes.Email);
        
        var article = await _articleService.Update(articleModel, userEmail);
        
        if(article is null)
            return NotFound();

        return Ok(article);
    }
    
    [HttpDelete("/{slug}")]
    [Authorize(Roles = "Author")]
    public async Task<IActionResult> Delete(string slug)
    {
        if(string.IsNullOrEmpty(slug))
            return BadRequest();
        
        await _articleService.Delete(slug);

        return Ok();
    }
    
}