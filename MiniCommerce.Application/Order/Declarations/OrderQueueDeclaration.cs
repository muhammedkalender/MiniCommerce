namespace MiniCommerce.Application.Order.Declarations;

public class OrderQueueDeclaration
{
    public static readonly TimeSpan Lag = TimeSpan.FromSeconds(2);
    
    public const string PlacedQueue = "order-placed";
}