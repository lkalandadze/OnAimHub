﻿namespace Shared.Infrastructure.MassTransit;

public class RabbitMqOptions
{
    public string Host { get; set; }
    public string ExchangeName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public Dictionary<string, string> RoutingKeys { get; set; }
}
