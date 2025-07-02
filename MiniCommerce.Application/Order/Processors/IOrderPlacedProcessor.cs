using MiniCommerce.Application.Order.DTOs;

namespace MiniCommerce.Application.Order.Processors;

public interface IOrderPlacedProcessor
{
    Task HandleAsync(OrderPlacedMessage message, Guid correlationId);
}