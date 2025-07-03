using System;
using Microsoft.Extensions.Logging;
using MiniCommerce.Domain.Notification.Enums;
using MiniCommerce.Domain.Notification.Models;
using MiniCommerce.Infrastructure.Notification.Services;
using Moq;
using NUnit.Framework;

namespace MiniCommerce.Infrastructure.Tests.Notification.Services;

[TestFixture]
[TestOf(typeof(LogNotificationService))]
public class LogNotificationServiceTest
{
    private Mock<ILogger<LogNotificationService>> _loggerMock = null;
    private LogNotificationService _service = null;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<LogNotificationService>>();
        _service = new LogNotificationService(_loggerMock.Object);
    }

    [Test]
    public void Send_Should_Log_Notification_And_Return_Same_Object()
    {
        // Arrange
        var notification = new NotificationModel
        {
            Title = "Test Title",
            Message = "Test Message",
            Target = Guid.NewGuid(),
            TargetType = NotificationTargetType.USER
        };

        // Act
        var result = _service.Send(notification);

        // Assert
        Assert.That(result, Is.EqualTo(notification));

        // Verify logger called once with correct values
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString()!.Contains("Simulated Notification") &&
                    v.ToString()!.Contains(notification.Title) &&
                    v.ToString()!.Contains(notification.Message) &&
                    v.ToString()!.Contains(notification.Target.ToString()) &&
                    v.ToString()!.Contains(notification.TargetType.ToString())),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}