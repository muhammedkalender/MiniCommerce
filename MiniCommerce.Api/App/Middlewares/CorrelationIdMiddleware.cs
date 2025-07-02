using MassTransit;

namespace MiniCommerce.Api.App.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationHeader = "x-correlation-id";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationHeader, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers[CorrelationHeader] = correlationId;
        }

        context.Items[CorrelationHeader] = correlationId;

        Serilog.Context.LogContext.PushProperty(MessageHeaders.CorrelationId, correlationId);
        
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationHeader] = correlationId;
            return Task.CompletedTask;
        });

        await _next(context);
    }
}