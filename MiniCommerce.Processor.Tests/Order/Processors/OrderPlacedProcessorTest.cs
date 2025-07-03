using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MiniCommerce.Application.Cache.Services;
using MiniCommerce.Application.Notification.Services;
using MiniCommerce.Application.Order.Declarations;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Domain.Notification.Enums;
using MiniCommerce.Domain.Notification.Models;
using MiniCommerce.Processor.Order.Processors;
using Moq;
using NUnit.Framework;

namespace MiniCommerce.Processor.Tests.Order.Processors;

[TestFixture]
[TestOf(typeof(OrderPlacedProcessor))]
public class OrderPlacedProcessorTest
{
    private Mock<ILogger<OrderPlacedProcessor>> _loggerMock;
    private Mock<ICacheService> _cacheServiceMock;
    private Mock<INotificationService> _notificationServiceMock;
    private OrderPlacedProcessor _processor;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<OrderPlacedProcessor>>();
        _cacheServiceMock = new Mock<ICacheService>();
        _notificationServiceMock = new Mock<INotificationService>();

        _processor = new OrderPlacedProcessor(
            _loggerMock.Object,
            _cacheServiceMock.Object,
            _notificationServiceMock.Object
        );
    }

    [Test]
    public async Task HandleAsync_Should_SetCache_Log_And_SendNotification()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var correlationId = Guid.NewGuid();

        var message = new OrderPlacedMessage
        {
            Id = orderId,
            UserId = userId
        };

        // Act
        await _processor.HandleAsync(message, correlationId);

        // Assert - Cache
        _cacheServiceMock.Verify(x => x.SetAsync(
            string.Format(OrderCacheDeclaration.ProcessedAtById, message.Id),
            It.IsAny<string>(),
            null
        ), Times.Once);

        // Assert - Logger
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains("Order processed") &&
                    v.ToString().Contains(orderId.ToString()) &&
                    v.ToString().Contains(correlationId.ToString())),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );

        // Assert - Notification
        _notificationServiceMock.Verify(x => x.Send(
                It.Is<NotificationModel>(n =>
                    n.Title == "Order placed" &&
                    n.Message.Contains("Order placed at") &&
                    n.Target == userId &&
                    n.TargetType == NotificationTargetType.USER)),
            Times.Once
        );
    }
}