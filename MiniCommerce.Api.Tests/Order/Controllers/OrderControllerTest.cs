using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Api.Order.Controllers;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Services;
using MiniCommerce.Domain.Payment.Enums;
using Moq;
using NUnit.Framework;

namespace MiniCommerce.Api.Tests.Order.Controllers;

[TestFixture]
[TestOf(typeof(OrderController))]
public class OrderControllerTest
{
    private Mock<IOrderService> _orderServiceMock = null!;
    private OrderController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new OrderController(_orderServiceMock.Object);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnOk_WithCreatedOrder()
    {
        // Arrange
        var request = new OrderCreateRequest
        {
            UserId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 2,
            PaymentMethod = PaymentMethod.BankTransfer
        };

        var expectedResponse = new OrderResponse
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            PaymentMethod = request.PaymentMethod,
            CreatedAt = DateTime.UtcNow
        };

        _orderServiceMock
            .Setup(s => s.CreateAsync(request))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(200));
        Assert.That(okResult.Value, Is.EqualTo(expectedResponse));
    }

    [Test]
    public async Task GetByUserIdAsync_ShouldReturnOk_WithOrderList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedOrders = new List<OrderResponse>
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                PaymentMethod = PaymentMethod.CreditCard,
                CreatedAt = DateTime.UtcNow
            }
        };

        _orderServiceMock
            .Setup(s => s.GetByUserIdAsync(userId))
            .ReturnsAsync(expectedOrders);

        // Act
        var result = await _controller.GetByUserIdAsync(userId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.StatusCode, Is.EqualTo(200));
        Assert.That(okResult.Value, Is.EqualTo(expectedOrders));
    }
}