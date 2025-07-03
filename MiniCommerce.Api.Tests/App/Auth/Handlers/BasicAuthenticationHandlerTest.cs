using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiniCommerce.Api.App.Auth.Handlers;
using MiniCommerce.Api.App.Auth.Signatures;
using Moq;
using NUnit.Framework;

namespace MiniCommerce.Api.Tests.App.Auth.Handlers;

[TestFixture]
[TestOf(typeof(BasicAuthenticationHandler))]
public class BasicAuthenticationHandlerTest
{
    private Mock<IBasicAuthenticationService> _authServiceMock;
    private AuthenticationSchemeOptions _schemeOptions;
    private Mock<ILoggerFactory> _loggerFactory;
    private UrlEncoder _urlEncoder;
    private Mock<ISystemClock> _clock;

    [SetUp]
    public void Setup()
    {
        _authServiceMock = new Mock<IBasicAuthenticationService>();
        _schemeOptions = new AuthenticationSchemeOptions();
        _loggerFactory = new Mock<ILoggerFactory>();
        _urlEncoder = UrlEncoder.Default;
        _clock = new Mock<ISystemClock>();
        
        var mockLogger = new Mock<ILogger>();
        _loggerFactory = new Mock<ILoggerFactory>();
        _loggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);
    }

    private BasicAuthenticationHandler CreateHandler(HttpContext context)
    {
        var options =
            Mock.Of<IOptionsMonitor<AuthenticationSchemeOptions>>(o => o.Get(It.IsAny<string>()) == _schemeOptions);

        var handler = new BasicAuthenticationHandler(
            options,
            _loggerFactory.Object,
            _urlEncoder,
            _clock.Object,
            _authServiceMock.Object);

        handler.InitializeAsync(new AuthenticationScheme("Basic", null, typeof(BasicAuthenticationHandler)), context)
            .Wait();

        return handler;
    }

    private string CreateBasicAuthHeader(string username, string password)
    {
        var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
        return "Basic " + Convert.ToBase64String(bytes);
    }

    [Test]
    public async Task Should_Authenticate_Valid_User()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var username = "admin";
        var password = "1234";

        context.Request.Headers["Authorization"] = CreateBasicAuthHeader(username, password);
        _authServiceMock.Setup(x => x.ValidateUser(username, password)).Returns(true);

        var handler = CreateHandler(context);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Principal.Identity.Name, Is.EqualTo(username));
    }

    [Test]
    public async Task Should_Fail_When_Invalid_Credentials()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = CreateBasicAuthHeader("invalid", "user");
        _authServiceMock.Setup(x => x.ValidateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        var handler = CreateHandler(context);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Failure.Message, Is.EqualTo("Invalid credentials"));
    }

    [Test]
    public async Task Should_Fail_When_Header_Is_Missing()
    {
        // Arrange
        var context = new DefaultHttpContext(); // No auth header
        var handler = CreateHandler(context);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Failure.Message, Is.EqualTo("Authorization header missing"));
    }

    [Test]
    public async Task Should_Fail_When_Header_Is_Invalid_Format()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Basic invalid_base64!";
        var handler = CreateHandler(context);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.Failure.Message, Is.EqualTo("Invalid Authorization header"));
    }
}