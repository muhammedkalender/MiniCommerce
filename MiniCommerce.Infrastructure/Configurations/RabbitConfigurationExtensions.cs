using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniCommerce.Infrastructure.Models;

namespace MiniCommerce.Infrastructure.Configurations;

public static class RabbitConfigurationExtensions
{
    public static IServiceCollection AddRabbitAsQueue(this IServiceCollection services, IConfiguration config,
        IEnumerable<RabbitConsumerDescriptor>? consumerDescriptors = null)
    {
        services.AddMassTransit(x =>
        {
            if (consumerDescriptors is not null)
            {
                foreach (var consumerDescriptor in consumerDescriptors)
                {
                    x.AddConsumer(consumerDescriptor.ConsumerType);
                }
            }


            x.UsingRabbitMq((context, cfg) =>
            {
                ushort.TryParse(config["Queues:RabbitMQ:Port"], out var port);

                cfg.Host(config["Queues:RabbitMQ:Host"], port, "/", h =>
                {
                    h.Username(config["Queues:RabbitMQ:User"] ?? throw new InvalidOperationException());
                    h.Password(config["Queues:RabbitMQ:Password"] ?? throw new InvalidOperationException());
                });

                if (consumerDescriptors is not null)
                {
                    foreach (var consumerDescriptor in consumerDescriptors)
                    {
                        cfg.ReceiveEndpoint(consumerDescriptor.QueueName,
                            e => { e.ConfigureConsumer(context, consumerDescriptor.ConsumerType); });
                    }
                }
            });
        });

        return services;
    }
}