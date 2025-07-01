using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using MiniCommerce.Api.App.Auth.Handlers;
using MiniCommerce.Api.App.Auth.Services;
using MiniCommerce.Api.App.Auth.Signatures;
using MiniCommerce.Api.App.Conventions;
using MiniCommerce.Api.Order.Validators;
using MiniCommerce.Application.Order.Repositories;
using MiniCommerce.Application.Order.Services;
using MiniCommerce.Infrastructure.Order.Repositories;
using MiniCommerce.Infrastructure.Order.Services;

namespace MiniCommerce.Api.App.Configurations;

public static class AppConfigurationExtensions
{
    public static IServiceCollection MapAppDependencies(this IServiceCollection services)
    {
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IOrderRepository, OrderRepository>();

        return services;
    }

    public static IServiceCollection ConfigureControllersAndJsonSettings(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.Conventions.Add(new SnakeCaseControllerNameConvention());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        return services;
    }

    public static IServiceCollection AddAndMapFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<OrderCreateRequestValidator>()
            .AddFluentValidationAutoValidation();

        return services;
    }

    public static IServiceCollection AddBasicAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("BasicAuth")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuth", null);

        services.AddScoped<IBasicAuthenticationService, BasicAuthenticationService>();

        return services;
    }
}