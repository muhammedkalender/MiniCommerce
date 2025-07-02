using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniCommerce.Application.Order.Declarations;
using MiniCommerce.Infrastructure.Cache.Configurations;
using MiniCommerce.Infrastructure.Log.Configurations;
using MiniCommerce.Infrastructure.Queue.Configurations;
using MiniCommerce.Processor.App.Configurations;
using MiniCommerce.Processor.Order.Consumers;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

SerilogConfiguration.ConfigureSerilogAsLogger(builder.Configuration);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

builder.Services.MapAppDependencies()
    .AddRedisAsCache(builder.Configuration)
    .AddRedisAsCache(builder.Configuration)
    .AddRabbitAsQueue(builder.Configuration, [
        new(typeof(OrderPlacedConsumer), OrderQueueDeclaration.PlacedQueue)
    ]);

var host = builder.Build();
host.Run();