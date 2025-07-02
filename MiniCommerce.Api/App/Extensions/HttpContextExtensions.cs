using MiniCommerce.Api.App.Declarations;

namespace MiniCommerce.Api.App.Extensions;

public static class HttpContextExtensions
{
    public static Guid? GetCorrelationId(this HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HttpDeclaration.CorrelationHeader, out var correlationId))
        {
            if (Guid.TryParse(correlationId, out var guid))
                return guid;
        }

        return null;
    }
}