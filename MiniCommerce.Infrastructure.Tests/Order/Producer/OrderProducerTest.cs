using System;
using System.Threading.Tasks;
using MassTransit.Testing;
using MiniCommerce.Application.App.Accessors;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Infrastructure.Order.Producer;
using Moq;
using NUnit.Framework;

namespace MiniCommerce.Infrastructure.Tests.Order.Producer;

[TestFixture]
[TestOf(typeof(OrderProducer))]
public class OrderProducerTest
{
    private InMemoryTestHarness _harness = null!;
    private OrderProducer _producer = null!;
    private Mock<ICorrelationIdAccessor> _correlationAccessorMock = null!;

    [SetUp]
    public async Task SetUp()
    {
        _harness = new InMemoryTestHarness();
        await _harness.Start();

        _correlationAccessorMock = new Mock<ICorrelationIdAccessor>();
        _correlationAccessorMock.Setup(x => x.GetCorrelationId())
            .Returns(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        var bus = _harness.Bus;

        _producer = new OrderProducer(bus, _correlationAccessorMock.Object);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _harness.Stop();
    }

    [Test]
    public async Task Should_Publish_OrderPlacedMessage_With_CorrelationId()
    {
        // Arrange
        var message = new OrderPlacedMessage
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };

        // Act
        await _producer.PublishOrderPlacedAsync(message);

        // Assert
        Assert.That(await _harness.Published.Any<OrderPlacedMessage>(), Is.True);

        var published = await _harness.Published.SelectAsync<OrderPlacedMessage>().First();

        Assert.That(published.Context.Message.Id, Is.EqualTo(message.Id));
        Assert.That(published.Context.Message.UserId, Is.EqualTo(message.UserId));
        Assert.That(published.Context.CorrelationId, Is.EqualTo(Guid.Parse("11111111-1111-1111-1111-111111111111")));
    }
}