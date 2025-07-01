﻿using MiniCommerce.Domain.Payment.Enums;

namespace MiniCommerce.Application.Order.DTOs;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime CreatedAt { get; set; }
}