using MiniCommerce.Domain.Notification.Models;

namespace MiniCommerce.Application.Notification.Services;

public interface INotificationService
{
    public NotificationModel Send(NotificationModel notification);
}