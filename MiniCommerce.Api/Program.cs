using MiniCommerce.Api.App.Configurations;
using MiniCommerce.Api.App.Middlewares;
using MiniCommerce.Infrastructure.Database.Configurations;
using MiniCommerce.Infrastructure.Queue.Configurations;
using MiniCommerce.Infrastructure.Log.Configurations;
using MiniCommerce.Infrastructure.Cache.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

SerilogConfiguration.ConfigureSerilogAsLogger(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();

builder.Services.MapAppDependencies()
    .AddAndMapFluentValidation()
    .ConfigureControllersAndJsonSettings()
    .AddSwaggerWithProjectContext()
    .AddBasicAuthentication()
    .AddRedisAsCache(builder.Configuration)
    .AddPostgresAsDb(builder.Configuration)
    .AddRabbitAsQueue(builder.Configuration);

var app = builder.Build();

// app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();