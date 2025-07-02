using MiniCommerce.Domain.Notification.Enums;

namespace MiniCommerce.Domain.Notification.Models;

public class NotificationModel
{
    public string Title { get; set; }
    public string Message { get; set; }
    public Guid? Target { get; set; }
    public NotificationTargetType TargetType { get; set; }
}