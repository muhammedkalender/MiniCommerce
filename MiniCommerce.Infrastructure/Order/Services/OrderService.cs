using AutoMapper;
using MiniCommerce.Application.App.Accessors;
using MiniCommerce.Application.Cache.Services;
using MiniCommerce.Application.Order.Declarations;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Producer;
using MiniCommerce.Application.Order.Repositories;
using MiniCommerce.Application.Order.Services;
using MiniCommerce.Domain.Order.Entities;

namespace MiniCommerce.Infrastructure.Order.Services;

[AutoConstructor]
public partial class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderProducer _orderProducer;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;

    public async Task<OrderResponse> CreateAsync(OrderCreateRequest request)
    {
        var order = _mapper.Map<OrderEntity>(request);
        order.Id = Guid.NewGuid();
        order.CreatedAt = DateTime.UtcNow;

        await _orderRepository.CreateAsync(order);

        await _orderProducer.PublishOrderPlacedAsync(_mapper.Map<OrderPlacedMessage>(order));
        await _cacheService.RemoveAsync(string.Format(OrderCacheDeclaration.OrdersByUser, request.UserId));

        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<IEnumerable<OrderResponse>> GetByUserIdAsync(Guid userId)
    {
        var cacheKey = string.Format(OrderCacheDeclaration.OrdersByUser, userId);
        var value = await _cacheService.GetAsync<IEnumerable<OrderResponse>>(cacheKey);

        if (value != null)
        {
            return value;
        }

        var orders = await _orderRepository.GetByUserIdAsync(userId);
        var mappedOrders = _mapper.Map<List<OrderResponse>>(orders);

        await _cacheService.SetAsync(cacheKey, mappedOrders, OrderCacheDeclaration.Lifetime);

        return mappedOrders;
    }
}