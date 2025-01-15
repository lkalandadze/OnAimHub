using FluentValidation;
using FluentValidation.AspNetCore;
using Leaderboard.Application.Behaviours;
using Leaderboard.Application.Consumers.Aggregation;
using Leaderboard.Application.Consumers.Play;
using Leaderboard.Application.Consumers.Players;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Infrastructure.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.MassTransit;
using Shared.IntegrationEvents.IntegrationEvents.Reward.Leaderboard;
using System.Reflection;

namespace Leaderboard.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                                                        Assembly.GetExecutingAssembly()));

        services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

        var assembly = Assembly.Load("Leaderboard.Application");
        services.AddScoped<IPlayerRepository, PlayerRepository>();
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
            x.AddConsumer<CreatePlayerAggregationConsumer>();
            x.AddConsumer<PlayLeaderboardAggregationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.User);
                    h.Password(rabbitMqOptions.Password);
                });

                cfg.Message<ReceiveLeaderboardRewardEvent>(c => c.SetEntityName("leaderboard.fanout"));
                cfg.Publish<ReceiveLeaderboardRewardEvent>(p =>
                {
                    p.ExchangeType = "fanout";
                });

                //var receiveLeaderQueueSettings = rabbitMqOptions.Queues["ReceiveLeaderboardRewardQueue"];
                //cfg.ReceiveEndpoint(receiveLeaderQueueSettings.QueueName, e =>
                //{
                //    var rabbitMqEndpoint = e as IRabbitMqReceiveEndpointConfigurator;
                //    foreach (var routingKey in receiveLeaderQueueSettings.RoutingKeys)
                //    {
                //        rabbitMqEndpoint?.Bind(rabbitMqOptions.ExchangeName, x =>
                //        {
                //            x.RoutingKey = routingKey;
                //            x.ExchangeType = "fanout";
                //        });
                //    }
                //    e.ConfigureConsumer<CreatePlayerAggregationConsumer>(context);
                //});

                var playerQueueSettings = rabbitMqOptions.Queues["CreatePlayerQueue"];
                cfg.ReceiveEndpoint(playerQueueSettings.QueueName, e =>
                {
                    var rabbitMqEndpoint = e as IRabbitMqReceiveEndpointConfigurator;
                    foreach (var routingKey in playerQueueSettings.RoutingKeys)
                    {
                        rabbitMqEndpoint?.Bind(rabbitMqOptions.ExchangeName, x =>
                        {
                            x.RoutingKey = routingKey;
                            x.ExchangeType = "fanout";
                        });
                    }
                    e.ConfigureConsumer<CreatePlayerAggregationConsumer>(context);
                });

                var playLeaderboardQueueSettings = rabbitMqOptions.Queues["PlayLeaderboardQueue"];
                cfg.ReceiveEndpoint(playLeaderboardQueueSettings.QueueName, e =>
                {
                    var rabbitMqEndpoint = e as IRabbitMqReceiveEndpointConfigurator;
                    foreach (var routingKey in playLeaderboardQueueSettings.RoutingKeys)
                    {
                        rabbitMqEndpoint?.Bind(rabbitMqOptions.ExchangeName, x =>
                        {
                            x.RoutingKey = routingKey;
                            x.ExchangeType = "fanout";
                        });
                    }
                    e.ConfigureConsumer<PlayLeaderboardAggregationConsumer>(context);
                });

                var leaderboardQueue = rabbitMqOptions.Queues["LeaderboardQueue"];
                cfg.ReceiveEndpoint(leaderboardQueue.QueueName, e =>
                {
                    var rabbitMqEndpoint = e as IRabbitMqReceiveEndpointConfigurator;
                    foreach(var routingKey in leaderboardQueue.RoutingKeys)
                    {
                        rabbitMqEndpoint?.Bind(rabbitMqOptions.ExchangeName, x =>
                        {
                            x.RoutingKey = routingKey;
                            x.ExchangeType = "fanout";
                        });
                    }
                    e.ConfigureConsumer<AggregatedEventConsumer>(context);
                });
            });
        });

        return services;
    }




}