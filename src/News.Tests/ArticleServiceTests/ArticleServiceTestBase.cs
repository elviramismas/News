using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Identity;
using Moq;
using News.Business.Services;
using News.Core.Entities;
using News.Core.Interfaces;

namespace News.Tests.ArticleServiceTests;

internal class ArticleServiceTestBase
{
    protected readonly Mock<IArticleRepository> MockedArticleRepository;
    protected readonly Mock<UserManager<ApplicationUser>> MockedUserManager;
    protected readonly IFixture Fixture;
    
    protected ArticleServiceTestBase()
    {
        MockedArticleRepository = new Mock<IArticleRepository>();
        MockedUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        Fixture = new Fixture().Customize(new AutoMoqCustomization());
    }

    protected ArticleService GetService()
    {
        return new ArticleService(MockedArticleRepository.Object, MockedUserManager.Object);
    }
}