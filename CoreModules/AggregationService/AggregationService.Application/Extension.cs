using AggregationService.Application.Services.Abstract;
using AggregationService.Application.Services.Concrete;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Bus;
using Shared.Infrastructure.MassTransit;
using Shared.IntegrationEvents.IntegrationEvents.Reward.Leaderboard;

namespace AggregationService.Application;

public static class Extension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAggregationConfigurationService, AggregationConfigurationService>();

        return services;
    }

    public static void AddMessageBus(this IServiceCollection services, IConfiguration configuration, Type consumerAssemblyMarkerType)
    {
        services.AddMassTransitWithRabbitMqTransport(configuration, consumerAssemblyMarkerType);
        services.AddScoped<IMessageBus, MessageBus>();
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
            //x.AddConsumer<CreatePlayerAggregationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.User);
                    h.Password(rabbitMqOptions.Password);
                });
            });
        });

        return services;
    }
}
