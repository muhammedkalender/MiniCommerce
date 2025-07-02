using Microsoft.Extensions.DependencyInjection;
using MiniCommerce.Domain.Order.Entities;
using MiniCommerce.Domain.Payment.Enums;
using MiniCommerce.Infrastructure.Database.Contexts;

namespace MiniCommerce.Infrastructure.Database.Initializers;

public static class MockDbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (context.Database.EnsureCreated())
        {
            SeedSampleData(context);
        }
    }

    private static void SeedSampleData(AppDbContext context)
    {
        if (context.Orders.Any()) return;

        var userIds = new[]
        {
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Guid.Parse("44444444-4444-4444-4444-444444444444")
        };

        var sampleOrders = new List<OrderEntity>
        {
            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[0], ProductId = Guid.NewGuid(), Quantity = 1,
                PaymentMethod = PaymentMethod.CreditCard, CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[0], ProductId = Guid.NewGuid(), Quantity = 2,
                PaymentMethod = PaymentMethod.BankTransfer, CreatedAt = DateTime.UtcNow.AddDays(-2)
            },

            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[1], ProductId = Guid.NewGuid(), Quantity = 3,
                PaymentMethod = PaymentMethod.CreditCard, CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[1], ProductId = Guid.NewGuid(), Quantity = 1,
                PaymentMethod = PaymentMethod.CreditCard, CreatedAt = DateTime.UtcNow.AddDays(-4)
            },

            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[2], ProductId = Guid.NewGuid(), Quantity = 4,
                PaymentMethod = PaymentMethod.BankTransfer, CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[2], ProductId = Guid.NewGuid(), Quantity = 2,
                PaymentMethod = PaymentMethod.CreditCard, CreatedAt = DateTime.UtcNow.AddDays(-6)
            },

            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[3], ProductId = Guid.NewGuid(), Quantity = 5,
                PaymentMethod = PaymentMethod.CreditCard, CreatedAt = DateTime.UtcNow.AddDays(-7)
            },
            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[3], ProductId = Guid.NewGuid(), Quantity = 3,
                PaymentMethod = PaymentMethod.BankTransfer, CreatedAt = DateTime.UtcNow.AddDays(-8)
            },

            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[0], ProductId = Guid.NewGuid(), Quantity = 2,
                PaymentMethod = PaymentMethod.BankTransfer, CreatedAt = DateTime.UtcNow.AddDays(-9)
            },
            new()
            {
                Id = Guid.NewGuid(), UserId = userIds[1], ProductId = Guid.NewGuid(), Quantity = 1,
                PaymentMethod = PaymentMethod.CreditCard, CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
        };

        context.Orders.AddRange(sampleOrders);

        context.SaveChanges();
    }
}