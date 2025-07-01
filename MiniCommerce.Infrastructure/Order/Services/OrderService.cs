using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Services;
using MiniCommerce.Domain.Order.Entities;

namespace MiniCommerce.Infrastructure.Order.Services;

public class OrderService : IOrderService
{
    public Task<OrderResponse> CreateAsync(OrderCreateRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderEntity>> GetByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}