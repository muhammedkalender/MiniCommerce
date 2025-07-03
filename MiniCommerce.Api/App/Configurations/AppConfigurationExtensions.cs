using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Api.App.Accessors;
using MiniCommerce.Api.App.Auth.Handlers;
using MiniCommerce.Api.App.Auth.Services;
using MiniCommerce.Api.App.Auth.Signatures;
using MiniCommerce.Api.App.Conventions;
using MiniCommerce.Api.App.Models;
using MiniCommerce.Api.Order.Validators;
using MiniCommerce.Application.App.Accessors;
using MiniCommerce.Application.Cache.Services;
using MiniCommerce.Application.Order.Producer;
using MiniCommerce.Application.Order.Repositories;
using MiniCommerce.Application.Order.Services;
using MiniCommerce.Infrastructure.Cache.Services;
using MiniCommerce.Infrastructure.Order.Producer;
using MiniCommerce.Infrastructure.Order.Repositories;
using MiniCommerce.Infrastructure.Order.Services;

namespace MiniCommerce.Api.App.Configurations;

public static class AppConfigurationExtensions
{
    public static IServiceCollection MapAppDependencies(this IServiceCollection services)
    {
        services.AddScoped<IOrderProducer, OrderProducer>();
        services.AddScoped<ICorrelationIdAccessor, HttpCorrelationIdAccessor>();

        services.AddTransient<ICacheService, RedisCacheService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IOrderRepository, OrderRepository>();

        return services;
    }

    public static IServiceCollection ConfigureControllersAndJsonSettings(this IServiceCollection services)
    {
        services.AddControllers(options => { options.Conventions.Add(new KebabCaseControllerNameConvention()); })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .Select(e => new
                        {
                            field = ToCamelCase(e.Key),
                            errors = e.Value?.Errors.Select(x => x.ErrorMessage)
                        });

                    return new BadRequestObjectResult(new ErrorModel()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Request validation failed",
                        Detail = errors
                    });
                };
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

    private static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
            return input;

        return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }
}