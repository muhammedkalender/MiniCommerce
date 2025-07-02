using MiniCommerce.Application.Cache.Services;
using MiniCommerce.Application.Notification.Services;
using MiniCommerce.Application.Order.Declarations;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Processors;
using MiniCommerce.Domain.Notification.Enums;
using MiniCommerce.Domain.Notification.Models;

namespace MiniCommerce.Proccesor.Order.Processors;

[AutoConstructor]
public partial class OrderPlacedProcessor : IOrderPlacedProcessor
{
    private readonly ILogger<OrderPlacedProcessor> _logger;
    private readonly ICacheService _cacheService;
    private readonly INotificationService _notificationService;

    public async Task HandleAsync(OrderPlacedMessage message, Guid correlationId)
    {
        await Task.Delay(OrderQueueDeclaration.Lag);

        var timestamp = DateTime.UtcNow.ToString("o");
        await _cacheService.SetAsync(string.Format(OrderCacheDeclaration.ProcessedAtById, message.Id), timestamp);

        _logger.LogInformation("Order processed: {OrderId} | CorrelationId: {CorrelationId}", message.Id,
            correlationId);

        _notificationService.Send(
            new()
            {
                Title = "Order placed",
                Message = $"Order placed at {timestamp}",
                Target = message.UserId,
                TargetType = NotificationTargetType.USER
            }
        );
    }
}