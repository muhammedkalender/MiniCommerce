namespace MiniCommerce.Infrastructure.Models;

public record RabbitConsumerDescriptor(Type ConsumerType, string QueueName);