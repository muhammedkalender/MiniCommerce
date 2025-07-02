using MassTransit;
using MiniCommerce.Application.App.Accessors;
using MiniCommerce.Application.Order.Declarations;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Producer;

namespace MiniCommerce.Infrastructure.Order.Producer;

[AutoConstructor]
public partial class OrderProducer : IOrderProducer
{
    private readonly IBus _bus;
    private readonly ICorrelationIdAccessor _correlationIdAccessor;

    public async Task PublishOrderPlacedAsync(OrderPlacedMessage order, Guid? correlationId = null)
    {
        var id = correlationId ?? _correlationIdAccessor.GetCorrelationId();

        await _bus.Publish(order, ctx =>
        {
            ctx.SetRoutingKey(OrderQueueDeclaration.PlacedQueue);
            ctx.CorrelationId = id;
        });
    }
}