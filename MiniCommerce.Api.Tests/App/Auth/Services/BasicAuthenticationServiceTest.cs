using MiniCommerce.Api.App.Auth.Services;
using NUnit.Framework;

namespace MiniCommerce.Api.Tests.App.Auth.Services;

[TestFixture]
[TestOf(typeof(BasicAuthenticationService))]
public class BasicAuthenticationServiceTest
{
    private BasicAuthenticationService _authService;

    [SetUp]
    public void Setup()
    {
        _authService = new BasicAuthenticationService();
    }

    [Test]
    public void ValidateUser_Should_Return_True_When_Credentials_Are_Correct()
    {
        // Arrange
        var username = "admin";
        var password = "1234";

        // Act
        var result = _authService.ValidateUser(username, password);

        // Assert
        Assert.That(result, Is.True);
    }

    [TestCase("admin", "wrongpass")]
    [TestCase("wronguser", "1234")]
    [TestCase("wronguser", "wrongpass")]
    [TestCase("", "")]
    [TestCase(null, null)]
    public void ValidateUser_Should_Return_False_When_Credentials_Are_Invalid(string username, string password)
    {
        // Act
        var result = _authService.ValidateUser(username, password);

        // Assert
        Assert.That(result, Is.False);
    }
}