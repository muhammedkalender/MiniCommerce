using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiniCommerce.Infrastructure.Cache.Configurations;

public static class RedisConfigurationExtensions
{
    public static IServiceCollection AddRedisAsCache(this IServiceCollection services, IConfiguration config)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{config["Caches:Redis:Host"]}:{config["Caches:Redis:Port"]}";
        });

        return services;
    }
}