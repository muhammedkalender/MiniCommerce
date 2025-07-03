using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Processors;
using MiniCommerce.Processor.Order.Consumers;
using Moq;
using NUnit.Framework;

namespace MiniCommerce.Processor.Tests.Order.Consumers;

[TestFixture]
[TestOf(typeof(OrderPlacedConsumer))]
public class OrderPlacedConsumerTest
{
    private Mock<ILogger<OrderPlacedConsumer>> _loggerMock;
    private Mock<IOrderPlacedProcessor> _processorMock;
    private OrderPlacedConsumer _consumer;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<OrderPlacedConsumer>>();
        _processorMock = new Mock<IOrderPlacedProcessor>();
        _consumer = new OrderPlacedConsumer(_loggerMock.Object, _processorMock.Object);
    }

    [Test]
    public async Task Consume_Should_ProcessOrder_And_Log()
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

        var contextMock = new Mock<ConsumeContext<OrderPlacedMessage>>();
        contextMock.Setup(x => x.Message).Returns(message);
        contextMock.Setup(x => x.CorrelationId).Returns(correlationId);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert - Processor Called
        _processorMock.Verify(x => x.HandleAsync(message, correlationId), Times.Once);

        // Assert - Logger logs
        VerifyLog(LogLevel.Information,
            $"Order received: {orderId} | CorrelationId: {correlationId}");

        VerifyLog(LogLevel.Information,
            $"Order passed to processor: {orderId}");
    }

    private void VerifyLog(LogLevel level, string contains)
    {
        _loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(contains)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeastOnce);
    }
}