using FluentValidation;
using FluentValidation.AspNetCore;
using Leaderboard.Application.Behaviours;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.MassTransit;
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
        var rabbitMqOptions = configuration.GetSection("MessageBroker:RabbitMQ").Get<RabbitMqOptions>();

        services.AddMassTransit(x =>
        {
            x.AddConsumers(consumerAssemblyMarkerType.Assembly);

            x.UsingRabbitMq(
                (context, cfg) =>
                {
                    cfg.Host(
                        rabbitMqOptions.Host,
                        h =>
                        {
                            h.Username(rabbitMqOptions.UserName);
                            h.Password(rabbitMqOptions.Password);
                        }
                    );

                    cfg.ReceiveEndpoint(
                        rabbitMqOptions.ExchangeName,
                        e =>
                        {
                            e.ConfigureConsumers(context);
                        }
                    );
                }
            );
        });

        return services; // Add this line
    }

}