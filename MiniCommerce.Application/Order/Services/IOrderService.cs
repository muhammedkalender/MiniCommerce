using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Domain.Order.Entities;

namespace MiniCommerce.Application.Order.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(OrderCreateRequest request);
    Task<IEnumerable<OrderResponse>> GetByUserIdAsync(Guid userId);
}