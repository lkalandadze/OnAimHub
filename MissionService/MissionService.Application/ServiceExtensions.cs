using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MissionService.Application.Behaviours;
using MissionService.Application.Consumers.Segment;
using Shared.Infrastructure.MassTransit;
using System.Reflection;

namespace MissionService.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                                                        Assembly.GetExecutingAssembly()));

        services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

        var assembly = Assembly.Load("MissionService.Application");

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
            x.AddConsumer<CreateSegmentAggregationConsumer>();
            x.AddConsumer<DeleteSegmentAggregationConsumer>();
            x.AddConsumer<UpdateSegmentAggregationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.User);
                    h.Password(rabbitMqOptions.Password);
                });

                var segmentQueueSettings = rabbitMqOptions.Queues["SegmentQueue"];
                cfg.ReceiveEndpoint(segmentQueueSettings.QueueName, e =>
                {
                    var rabbitMqEndpoint = e as IRabbitMqReceiveEndpointConfigurator;

                    foreach (var routingKey in segmentQueueSettings.RoutingKeys)
                    {
                        rabbitMqEndpoint?.Bind(rabbitMqOptions.ExchangeName, x =>
                        {
                            x.RoutingKey = routingKey;
                            x.ExchangeType = "fanout";
                        });
                    }

                    e.ConfigureConsumer<CreateSegmentAggregationConsumer>(context);
                    e.ConfigureConsumer<DeleteSegmentAggregationConsumer>(context);
                    e.ConfigureConsumer<UpdateSegmentAggregationConsumer>(context);
                });
            });
        });

        return services;
    }
}