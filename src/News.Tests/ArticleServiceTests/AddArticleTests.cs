using AutoFixture;
using Moq;
using News.Core.Entities;
using News.Core.Models;

namespace News.Tests.ArticleServiceTests;

internal class AddArticleTests : ArticleServiceTestBase
{
    [Test]
    public async Task AddSuccessfulCase_ShouldReturnAddedArticle()
    {
        // Arrange
        var email = Fixture.Create<string>();
        var user = Fixture.Build<ApplicationUser>()
            .Without(x => x.Articles)
            .Create();
        var articleModel = Fixture.Build<ArticleModel>()
            .Without(x => x.Slug)
            .Create();
        var articleEntity = articleModel.ToEntity();
        
        MockedUserManager.Setup(x => x.FindByEmailAsync(It.Is<string>(x => x == email)))
            .ReturnsAsync(user);
        MockedArticleRepository.Setup(x => x.Add(It.IsAny<Article>()))
            .ReturnsAsync(articleEntity);
        
        // Act
        var addedArticle = await GetService().Add(articleModel, email);

        // Assert
        Assert.That(addedArticle.Slug, Is.EqualTo(articleEntity.Slug));
    }
}