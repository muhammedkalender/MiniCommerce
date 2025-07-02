using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Application.Order.Processors;

namespace MiniCommerce.Processor.Order.Consumers;

[AutoConstructor]
public partial class OrderPlacedConsumer : IConsumer<OrderPlacedMessage>
{
    private readonly ILogger<OrderPlacedConsumer> _logger;
    private readonly IOrderPlacedProcessor _processor;

    public async Task Consume(ConsumeContext<OrderPlacedMessage> context)
    {
        var message = context.Message;
        var correlationId = context.CorrelationId ?? Guid.Empty;
        
        using (Serilog.Context.LogContext.PushProperty(MessageHeaders.CorrelationId, correlationId))
        {
            _logger.LogInformation("Order received: {OrderId} | CorrelationId: {CorrelationId}", message.Id,
                correlationId);

            await _processor.HandleAsync(message, correlationId);

            _logger.LogInformation("Order passed to processor: {OrderId}", message.Id);
        }
    }
}