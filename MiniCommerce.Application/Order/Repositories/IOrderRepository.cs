using MiniCommerce.Domain.Order.Entities;

namespace MiniCommerce.Application.Order.Repositories;

public interface IOrderRepository
{
    public Task<OrderEntity> CreateAsync(OrderEntity entity);
    Task<IEnumerable<OrderEntity>> GetByUserIdAsync(Guid userId);
}