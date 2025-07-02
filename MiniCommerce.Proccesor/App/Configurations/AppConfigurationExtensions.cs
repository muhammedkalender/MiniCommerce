using MiniCommerce.Application.Cache.Services;
using MiniCommerce.Application.Notification.Services;
using MiniCommerce.Application.Order.Processors;
using MiniCommerce.Infrastructure.Cache.Services;
using MiniCommerce.Infrastructure.Notification.Services;
using MiniCommerce.Proccesor.Order.Processors;

namespace MiniCommerce.Proccesor.App.Configurations;

public static class AppConfigurationExtensions
{
    public static IServiceCollection MapAppDependencies(this IServiceCollection services)
    {
        services.AddTransient<INotificationService, LogNotificationService>();
        services.AddTransient<ICacheService, RedisCacheService>();
        services.AddTransient<IOrderPlacedProcessor, OrderPlacedProcessor>();

        return services;
    }
}