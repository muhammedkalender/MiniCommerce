using MassTransit;
using MiniCommerce.Application.Order.Declarations;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Producer;

namespace MiniCommerce.Infrastructure.Order.Producer;

public class OrderProducer : IOrderProducer
{
    private readonly IBus _bus;

    public async Task PublishOrderPlacedAsync(OrderPlacedMessage order, Guid? correlationId = null)
    {
        var id = correlationId ?? Guid.NewGuid();

        await _bus.Publish(order, ctx =>
        {
            ctx.SetRoutingKey(OrderQueueDeclaration.OrderPlacedQueue);
            ctx.CorrelationId = id;
        });
    }
}