using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MiniCommerce.Api.App.Declarations;
using MiniCommerce.Api.App.Middlewares;
using NUnit.Framework;

namespace MiniCommerce.Api.Tests.App.Middlewares;

[TestFixture]
[TestOf(typeof(CorrelationIdMiddleware))]
public class CorrelationIdMiddlewareTest
{
    [Test]
    public async Task Should_Add_CorrelationId_When_Not_Exists()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var called = false;
        var middleware = new CorrelationIdMiddleware(ctx =>
        {
            called = true;
            return Task.CompletedTask;
        });

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.That(called, Is.True);
        Assert.That(context.Request.Headers.ContainsKey(HttpDeclaration.CorrelationHeader), Is.True);
        Assert.That(context.Response.Headers.ContainsKey(HttpDeclaration.CorrelationHeader), Is.True);
        Assert.That(context.Items.ContainsKey(HttpDeclaration.CorrelationHeader), Is.True);
    }

    [Test]
    public async Task Should_Use_Existing_CorrelationId()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var existingCorrelationId = Guid.NewGuid().ToString();
        context.Request.Headers[HttpDeclaration.CorrelationHeader] = new StringValues(existingCorrelationId);

        var middleware = new CorrelationIdMiddleware(ctx => Task.CompletedTask);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.That(context.Response.Headers[HttpDeclaration.CorrelationHeader].ToString(), Is.EqualTo(existingCorrelationId));
    }
}