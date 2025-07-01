using MiniCommerce.Application.Order.Repositories;
using MiniCommerce.Domain.Order.Entities;

namespace MiniCommerce.Infrastructure.Order.Repositories;

public class OrderRepository : IOrderRepository
{
    public Task<OrderEntity> CreateAsync(OrderEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderEntity>> GetByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}