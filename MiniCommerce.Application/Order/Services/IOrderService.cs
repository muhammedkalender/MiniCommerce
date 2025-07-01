using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Domain.Order.Entities;

namespace MiniCommerce.Application.Order.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(OrderCreateRequest request);
    Task<IEnumerable<OrderEntity>> GetByUserIdAsync(Guid userId);
}