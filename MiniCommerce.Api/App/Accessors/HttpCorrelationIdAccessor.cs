using MiniCommerce.Api.App.Declarations;
using MiniCommerce.Application.App.Accessors;

namespace MiniCommerce.Api.App.Accessors;

[AutoConstructor]
public partial class HttpCorrelationIdAccessor : ICorrelationIdAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Guid GetCorrelationId()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context != null &&
            context.Request.Headers.TryGetValue(HttpDeclaration.CorrelationHeader, out var correlationId) &&
            Guid.TryParse(correlationId, out var guid))
        {
            return guid;
        }

        return Guid.NewGuid(); // fallback
    }
}