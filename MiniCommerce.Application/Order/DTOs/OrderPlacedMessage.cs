namespace MiniCommerce.Application.Order.DTOs;

public class OrderPlacedMessage
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}