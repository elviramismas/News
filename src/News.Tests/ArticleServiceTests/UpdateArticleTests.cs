using System.Security;
using AutoFixture;
using Moq;
using News.Core.Entities;
using News.Core.Models;

namespace News.Tests.ArticleServiceTests;

internal class UpdateArticleTests : ArticleServiceTestBase
{
    [Test]
    public void UserDoesNotHaveAccessCase_ShouldThrowSecurityException()
    { 
        // Arrange
        var email = Fixture.Create<string>();
        var user = Fixture.Build<ApplicationUser>()
            .Without(x => x.Articles)
            .Create();
        var articleModel = Fixture.Build<ArticleModel>()
            .Without(x => x.Slug)
            .Create();
        var articleEntity = Fixture.Build<Article>()
            .Without(x => x.Author)
            .Create();

        MockedArticleRepository.Setup(x => x.GetBySlug(It.IsAny<string>()))
            .ReturnsAsync(articleEntity);
        MockedUserManager.Setup(x => x.FindByEmailAsync(It.Is<string>(x => x == email)))
            .ReturnsAsync(user);
        
        // Act and assert
        Assert.ThrowsAsync<SecurityException>(async () => await GetService().Update(articleModel, email));
    }
    
    [Test]
    public async Task UpdateSuccessfulCase_ShouldReturnUpdatedArticle()
    {
        // Arrange
        var email = Fixture.Create<string>();
        var user = Fixture.Build<ApplicationUser>()
            .Without(x => x.Articles)
            .Create();
        var articleModel = Fixture.Build<ArticleModel>()
            .Without(x => x.Slug)
            .Create();
        var articleEntity = Fixture.Build<Article>()
            .Without(x => x.Author)
            .Create();
        
        articleEntity.AuthorId = user.Id;
        
        MockedArticleRepository.Setup(x => x.GetBySlug(It.IsAny<string>()))
            .ReturnsAsync(articleEntity);
        MockedUserManager.Setup(x => x.FindByEmailAsync(It.Is<string>(x => x == email)))
            .ReturnsAsync(user);
        MockedArticleRepository.Setup(x => x.Update(It.IsAny<Article>(), It.IsAny<string>()))
            .ReturnsAsync(articleEntity);
        
        // Act
        var updatedArticle = await GetService().Update(articleModel, email);
        
        // Assert
        Assert.That(updatedArticle, Is.Not.Null);
    }
}