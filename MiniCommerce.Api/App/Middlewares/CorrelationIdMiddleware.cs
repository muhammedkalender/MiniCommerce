using MassTransit;
using MiniCommerce.Api.App.Declarations;

namespace MiniCommerce.Api.App.Middlewares;

[AutoConstructor]
public partial class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[HttpDeclaration.CorrelationHeader].FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers[HttpDeclaration.CorrelationHeader] = correlationId;
        }

        using (Serilog.Context.LogContext.PushProperty(MessageHeaders.CorrelationId, correlationId))
        {
            context.Response.Headers[HttpDeclaration.CorrelationHeader] = correlationId;
            context.Items[HttpDeclaration.CorrelationHeader] = correlationId;

            await _next(context);
        }
    }
}