namespace Shared.Infrastructure.MassTransit;

public class RabbitMqOptions
{
    public string Host { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string ExchangeName { get; set; }
    public Dictionary<string, QueueSettings> Queues { get; set; }
}

public class QueueSettings
{
    public string QueueName { get; set; }
    public List<string> RoutingKeys { get; set; } = new List<string>();
}


