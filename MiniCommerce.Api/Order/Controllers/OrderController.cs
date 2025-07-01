using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Services;

namespace MiniCommerce.Api.Order.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] OrderCreateRequest request)
    {
        var result = await _orderService.CreateAsync(request);
        return Ok(result);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserIdAsync(Guid userId)
    {
        var orders = await _orderService.GetByUserIdAsync(userId);
        return Ok(orders);
    }
}