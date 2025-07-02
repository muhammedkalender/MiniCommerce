using MiniCommerce.Application.Order.DTOs;

namespace MiniCommerce.Application.Order.Producer;

public interface IOrderProducer
{
    Task PublishOrderPlacedAsync(OrderPlacedMessage order, Guid? correlationId = null);
}