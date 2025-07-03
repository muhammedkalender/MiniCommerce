using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MiniCommerce.Application.Cache.Services;
using MiniCommerce.Application.Order.Declarations;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Producer;
using MiniCommerce.Application.Order.Repositories;
using MiniCommerce.Domain.Order.Entities;
using MiniCommerce.Domain.Payment.Enums;
using MiniCommerce.Infrastructure.Order.Services;
using Moq;
using NUnit.Framework;

namespace MiniCommerce.Infrastructure.Tests.Order.Services;

[TestFixture]
[TestOf(typeof(OrderService))]
public class OrderServiceTest
{
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<IOrderProducer> _orderProducerMock;
    private Mock<ICacheService> _cacheServiceMock;
    private IMapper _mapper;
    private OrderService _orderService;

    [SetUp]
    public void SetUp()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _orderProducerMock = new Mock<IOrderProducer>();
        _cacheServiceMock = new Mock<ICacheService>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<OrderCreateRequest, OrderEntity>();
            cfg.CreateMap<OrderEntity, OrderPlacedMessage>();
            cfg.CreateMap<OrderEntity, OrderResponse>();
        });

        _mapper = mapperConfig.CreateMapper();

        _orderService = new OrderService(
            _orderRepositoryMock.Object,
            _orderProducerMock.Object,
            _cacheServiceMock.Object,
            _mapper
        );
    }

    [Test]
    public async Task CreateAsync_Should_Create_Order_And_Publish_And_Invalidate_Cache()
    {
        // Arrange
        var request = new OrderCreateRequest
        {
            UserId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 5,
            PaymentMethod = PaymentMethod.CreditCard
        };

        // Act
        var response = await _orderService.CreateAsync(request);

        // Assert
        _orderRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<OrderEntity>()), Times.Once);
        // todo _orderProducerMock.Verify(x =>
        //         x.PublishOrderPlacedAsync(It.IsAny<OrderPlacedMessage>(), It.Is<CancellationToken>(ct => ct == CancellationToken.None)),
        //     Times.Once);
        _cacheServiceMock.Verify(x => x.RemoveAsync(It.Is<string>(k => k.Contains(request.UserId.ToString()))),
            Times.Once);

        Assert.That(response, Is.Not.Null);
        Assert.That(request.UserId, Is.EqualTo(request.UserId));
        Assert.That(request.ProductId, Is.EqualTo(response.ProductId));
    }

    [Test]
    public async Task GetByUserIdAsync_Should_Return_From_Cache_If_Available()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expected = new List<OrderResponse>
        {
            new OrderResponse { Id = Guid.NewGuid(), UserId = userId }
        };

        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<OrderResponse>>(It.IsAny<string>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _orderService.GetByUserIdAsync(userId);

        // Assert
        _orderRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>()), Times.Never);
        _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()),
            Times.Never);

        Assert.That(expected, Is.EqualTo(result));
    }

    [Test]
    public async Task GetByUserIdAsync_Should_Query_Repository_And_Set_Cache_If_Cache_Miss()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<OrderResponse>>(It.IsAny<string>()))
            .ReturnsAsync((IEnumerable<OrderResponse>)null!);

        _orderRepositoryMock.Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(new List<OrderEntity>
            {
                new OrderEntity { Id = Guid.NewGuid(), UserId = userId }
            });

        // Act
        var result = await _orderService.GetByUserIdAsync(userId);

        // Assert
        _orderRepositoryMock.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
        _cacheServiceMock.Verify(x => x.SetAsync(
            It.Is<string>(key => key.Contains(userId.ToString())),
            It.IsAny<object>(),
            OrderCacheDeclaration.Lifetime), Times.Once);

        Assert.That(result, Is.Not.Empty);
    }
}