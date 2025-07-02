namespace MiniCommerce.Infrastructure.Queue.Models;

public record RabbitConsumerDescriptor(Type ConsumerType, string QueueName);