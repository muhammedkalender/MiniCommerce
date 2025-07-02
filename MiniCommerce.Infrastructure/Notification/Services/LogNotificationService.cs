using Microsoft.Extensions.Logging;
using MiniCommerce.Application.Notification.Services;
using MiniCommerce.Domain.Notification.Models;

namespace MiniCommerce.Infrastructure.Notification.Services;

[AutoConstructor]
public partial class LogNotificationService : INotificationService
{
    private readonly ILogger<LogNotificationService> _logger;
    
    public NotificationModel Send(NotificationModel notification)
    {
        _logger.LogInformation("Simulated Notification | Title: {Title}, Message: {Message}, Target: {Target}, TargetType: {TargetType}",
            notification.Title,
            notification.Message,
            notification.Target,
            notification.TargetType);

        return notification;
    }
}