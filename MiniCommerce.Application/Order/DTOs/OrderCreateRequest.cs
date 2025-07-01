using MiniCommerce.Domain.Payment.Enums;

namespace MiniCommerce.Application.Order.DTOs;

public class OrderCreateRequest
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}