using Microsoft.EntityFrameworkCore;
using MiniCommerce.Application.Order.Repositories;
using MiniCommerce.Domain.Order.Entities;
using MiniCommerce.Infrastructure.Contexts;

namespace MiniCommerce.Infrastructure.Order.Repositories;

[AutoConstructor]
public partial class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public async Task<OrderEntity> CreateAsync(OrderEntity entity)
    {
        _context.Orders.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<OrderEntity>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }
}