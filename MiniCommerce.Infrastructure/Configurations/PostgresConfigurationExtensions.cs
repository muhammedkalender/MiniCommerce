using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniCommerce.Infrastructure.Contexts;

namespace MiniCommerce.Infrastructure.Configurations;

public static class PostgresConfigurationExtensions
{
    public static IServiceCollection AddPostgresAsDb(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        return services;
    }
}