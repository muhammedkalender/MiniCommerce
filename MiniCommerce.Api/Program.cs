using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using MiniCommerce.Api.App.Auth.Handlers;
using MiniCommerce.Api.App.Auth.Services;
using MiniCommerce.Api.App.Auth.Signatures;
using MiniCommerce.Api.App.Middlewares;
using MiniCommerce.Api.Order.Validators;
using MiniCommerce.Application.Order.Services;
using MiniCommerce.Infrastructure.Order.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddValidatorsFromAssemblyContaining<OrderCreateRequestValidator>()
    .AddFluentValidationAutoValidation();

builder.Services.AddAuthentication("BasicAuth")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuth", null);

builder.Services.AddControllers();
builder.Services.AddScoped<IBasicAuthenticationService, BasicAuthenticationService>();
builder.Services.AddTransient<IOrderService, OrderService>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MiniCommerce API",
        Version = "v1"
    });

    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authentication header"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            []
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.UseAuthorization();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();