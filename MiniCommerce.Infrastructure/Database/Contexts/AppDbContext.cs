using Microsoft.EntityFrameworkCore;
using MiniCommerce.Domain.Order.Entities;

namespace MiniCommerce.Infrastructure.Database.Contexts;

public class AppDbContext: DbContext
{
    public DbSet<OrderEntity> Orders { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
}