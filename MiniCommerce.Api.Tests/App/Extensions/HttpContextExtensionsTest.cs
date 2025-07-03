using System;
using Microsoft.AspNetCore.Http;
using MiniCommerce.Api.App.Declarations;
using MiniCommerce.Api.App.Extensions;
using NUnit.Framework;

namespace MiniCommerce.Api.Tests.App.Extensions;

[TestFixture]
[TestOf(typeof(HttpContextExtensions))]
public class HttpContextExtensionsTest
{
    [Test]
    public void Should_Return_Guid_When_Header_Is_Valid()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var expectedId = Guid.NewGuid().ToString();
        context.Request.Headers[HttpDeclaration.CorrelationHeader] = expectedId;

        // Act
        var result = context.GetCorrelationId();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ToString(), Is.EqualTo(expectedId));
    }

    [Test]
    public void Should_Return_Null_When_Header_Is_Missing()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act
        var result = context.GetCorrelationId();

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Should_Return_Null_When_Header_Is_Invalid_Guid()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers[HttpDeclaration.CorrelationHeader] = "not-a-guid";

        // Act
        var result = context.GetCorrelationId();

        // Assert
        Assert.That(result, Is.Null);
    }
}