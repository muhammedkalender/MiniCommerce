using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniCommerce.Domain.Order.Entities;
using MiniCommerce.Domain.Payment.Enums;
using MiniCommerce.Infrastructure.Database.Contexts;
using MiniCommerce.Infrastructure.Order.Repositories;
using NUnit.Framework;

namespace MiniCommerce.Infrastructure.Tests.Order.Repositories;

[TestFixture]
[TestOf(typeof(OrderRepository))]
public class OrderRepositoryTest
{
    private AppDbContext _context = null!;
    private OrderRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _repository = new OrderRepository(_context);
    }

    [Test]
    public async Task CreateAsync_Should_Add_Entity_To_Database()
    {
        // Arrange
        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 3,
            PaymentMethod = PaymentMethod.CreditCard,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _repository.CreateAsync(order);

        // Assert
        var stored = await _context.Orders.FindAsync(order.Id);
        Assert.That(stored, Is.Not.Null);
        Assert.That(stored.UserId, Is.EqualTo(order.UserId));
        Assert.That(result.Id, Is.EqualTo(order.Id));
    }

    [Test]
    public async Task GetByUserIdAsync_Should_Return_Only_Users_Orders()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var orders = new List<OrderEntity>
        {
            new OrderEntity
            {
                Id = Guid.NewGuid(), UserId = userId, ProductId = Guid.NewGuid(), Quantity = 1,
                CreatedAt = DateTime.UtcNow
            },
            new OrderEntity
            {
                Id = Guid.NewGuid(), UserId = userId, ProductId = Guid.NewGuid(), Quantity = 2,
                CreatedAt = DateTime.UtcNow
            },
            new OrderEntity
            {
                Id = Guid.NewGuid(), UserId = otherUserId, ProductId = Guid.NewGuid(), Quantity = 3,
                CreatedAt = DateTime.UtcNow
            }
        };

        _context.Orders.AddRange(orders);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUserIdAsync(userId);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.All(x => x.UserId == userId), Is.True);
    }
}