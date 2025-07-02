namespace MiniCommerce.Application.App.Accessors;

public interface ICorrelationIdAccessor
{
    Guid GetCorrelationId();
}