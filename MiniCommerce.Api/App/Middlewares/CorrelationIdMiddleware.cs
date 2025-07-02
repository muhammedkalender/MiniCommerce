using MassTransit;
using MiniCommerce.Api.App.Declarations;

namespace MiniCommerce.Api.App.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(HttpDeclaration.CorrelationHeader, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers[HttpDeclaration.CorrelationHeader] = correlationId;
        }

        context.Items[HttpDeclaration.CorrelationHeader] = correlationId;

        Serilog.Context.LogContext.PushProperty(MessageHeaders.CorrelationId, correlationId);
        
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HttpDeclaration.CorrelationHeader] = correlationId;
            return Task.CompletedTask;
        });

        await _next(context);
    }
}