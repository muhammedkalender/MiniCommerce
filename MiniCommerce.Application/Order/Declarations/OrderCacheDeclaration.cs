namespace MiniCommerce.Application.Order.Declarations;

public class OrderCacheDeclaration
{
    public static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(2);
    
    public const string OrdersByUser = "orders_by_user:{0}";
    public const string OrderById = "orders_by_id:{0}";
    public const string ProcessedAtById = "order_processed_time_by_id:{0}";
}