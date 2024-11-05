using FluentValidation;
using FluentValidation.AspNetCore;
using LevelService.Application.Behaviours;
using LevelService.Application.Consumers.Spins;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.MassTransit;
using System.Reflection;

namespace LevelService.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                                                        Assembly.GetExecutingAssembly()));

        services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

        var assembly = Assembly.Load("LevelService.Application");

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

        return services;
    }

    public static IServiceCollection AddMassTransitWithRabbitMqTransport(
    this IServiceCollection services,
    IConfiguration configuration,
    Type consumerAssemblyMarkerType
    )
    {
        var rabbitMqOptions = configuration.GetSection("RabbitMQSettings").Get<RabbitMqOptions>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<UpdatePlayerExperienceAggregationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.User);
                    h.Password(rabbitMqOptions.Password);
                });

                var playerExperienceQueueSettings = rabbitMqOptions.Queues["UpdatePlayerExperienceQueue"];
                cfg.ReceiveEndpoint(playerExperienceQueueSettings.QueueName, e =>
                {
                    var rabbitMqEndpoint = e as IRabbitMqReceiveEndpointConfigurator;

                    foreach (var routingKey in playerExperienceQueueSettings.RoutingKeys)
                    {
                        rabbitMqEndpoint?.Bind(rabbitMqOptions.ExchangeName, x =>
                        {
                            x.RoutingKey = routingKey;
                            x.ExchangeType = "fanout";
                        });
                    }

                    e.ConfigureConsumer<UpdatePlayerExperienceAggregationConsumer>(context);
                });
            });
        });

        return services;
    }
}