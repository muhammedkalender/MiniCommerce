using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniCommerce.Api.App.Middlewares;
using MiniCommerce.Api.App.Models;
using Moq;
using NUnit.Framework;

namespace MiniCommerce.Api.Tests.App.Middlewares;

[TestFixture]
[TestOf(typeof(ExceptionMiddleware))]
public class ExceptionMiddlewareTest
{
    private Mock<ILogger<ExceptionMiddleware>> _loggerMock = null!;
    private Mock<IHostEnvironment> _envMock = null!;
    private DefaultHttpContext _context = null!;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        _envMock = new Mock<IHostEnvironment>();
        _context = new DefaultHttpContext();
    }

    [Test]
    public async Task Should_Return_500_When_Unhandled_Exception_Occurs()
    {
        // Arrange
        _envMock.Setup(e => e.EnvironmentName).Returns("Production"); // çünkü IsDevelopment false olacak
        var middleware =
            new ExceptionMiddleware(_ => throw new Exception("Boom!"), _loggerMock.Object, _envMock.Object);

        var responseStream = new MemoryStream();
        _context.Response.Body = responseStream;

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        var error = JsonSerializer.Deserialize<ErrorModel>(body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.That(_context.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        Assert.That(error?.Message, Is.EqualTo("An unexpected error occurred."));
    }

    [Test]
    public async Task Should_Return_400_When_Status_Is_Already_Set()
    {
        // Arrange
        var middleware = new ExceptionMiddleware(async context => { context.Response.StatusCode = 400; },
            _loggerMock.Object, _envMock.Object);

        var responseStream = new MemoryStream();
        _context.Response.Body = responseStream;

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        var error = JsonSerializer.Deserialize<ErrorModel>(body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.That(_context.Response.StatusCode, Is.EqualTo(400));
        Assert.That(error?.Message, Is.EqualTo("Bad Request"));
    }
}