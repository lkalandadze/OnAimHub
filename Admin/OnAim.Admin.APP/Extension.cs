﻿using OnAim.Admin.Contracts.Models;
using OnAim.Admin.APP.Feature.Identity;
using OnAim.Admin.Contracts.Helpers.Csv;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Configuration;
using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.APP.Services.LeaderBoard;
using OnAim.Admin.APP.Services.Admin.Domain;
using OnAim.Admin.APP.Services.Admin.EmailServices;
using OnAim.Admin.APP.Services.Admin.Endpoint;
using OnAim.Admin.APP.Services.Admin.EndpointGroup;
using OnAim.Admin.APP.Services.Admin.Role;
using OnAim.Admin.APP.Services.Admin.SettingServices;
using OnAim.Admin.APP.Services.Admin.User;
using OnAim.Admin.APP.Services.Admin.AuthServices;
using OnAim.Admin.APP.Services.Hub.Player;
using OnAim.Admin.APP.Services.Hub.Promotion;
using OnAim.Admin.APP.Services.Hub.Segment;
using OnAim.Admin.APP.Services.Hub.Coin;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.APP.Services.LeaderBoardServices;
using OnAim.Admin.APP.Services.AdminServices.Domain;
using OnAim.Admin.APP.Services.AdminServices.EmailServices;
using OnAim.Admin.APP.Services.AdminServices.Endpoint;
using OnAim.Admin.APP.Services.AdminServices.EndpointGroup;
using OnAim.Admin.APP.Services.AdminServices.Role;
using OnAim.Admin.APP.Services.AdminServices.User;


namespace OnAim.Admin.APP;

public static class Extension
{
    public static IServiceCollection AddApp(
        this IServiceCollection services,
        IConfiguration configuration,
        Type consumerAssemblyMarkerType,
        Action<EmailOptions>? configureOptions = null)
    {
        var emailOptions = new EmailOptions();
        configuration.GetSection(nameof(EmailOptions)).Bind(emailOptions);
        configureOptions?.Invoke(emailOptions);
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        services.AddSingleton<LeaderboardClientService>(sp =>
        new LeaderboardClientService("http://192.168.88.138:8002", new HttpClient()));
        services
            .AddScoped<IGameTemplateService, GameTemplateService>()
            .AddScoped<IPromotionService, PromotionService>()
            .AddScoped<ICoinTemplateService, CoinTemplateService>()
            .AddScoped<IPermissionService, PermissionService>()
            .AddScoped<ILeaderboardTemplateService, LeaderboardTemplateService>()
            .AddScoped<IPasswordService, PasswordService>()
            .AddScoped<IPromotionViewTemplateService, PromotionViewTemplateService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IEndpointService, EndpointService>()
            .AddScoped<IEndpointGroupService, EndpointGroupService>()
            .AddScoped<IRoleService, RoleService>()
            .AddScoped<ICoinService, CoinService>()
            .AddScoped<IDomainService, DomainService>()
            .AddScoped<IFileManagementService, FileManagementService>()
            .AddScoped<IGameService, GameService>()
            .AddScoped<IPromotionTemplateService, PromotionTemplateService>()
            .AddScoped<ISegmentService, SegmentService>()
            .AddScoped<ILeaderBoardService, LeaderBoardService>()
            .AddScoped<IPlayerService, PlayerService>()
            .AddTransient<IJwtFactory, JwtFactory>()
            .AddHostedService<TokenCleanupService>()
            .Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"))
            .AddScoped<IDomainValidationService, DomainValidationService>()
            .AddScoped<AppSettings>()
            .AddScoped<IOtpService, OtpService>()
            .AddScoped(typeof(ICsvWriter<>), typeof(CsvWriter<>))
            .AddScoped(typeof(CommandContext<>), typeof(CommandContext<>));

        services.AddHttpClient("ApiGateway", client =>
        {
            client.BaseAddress = new Uri("http://ocelotapigateway:8080");
            var basicAuthToken = Convert.ToBase64String(Encoding.ASCII.GetBytes("a:a"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthToken);
        });
        services.Configure<PostmarkOptions>(configuration.GetSection("Postmark"));
        services.Configure<PromotionViewConfiguration>(configuration.GetSection("PromotionViewConfiguration"));
        services.AddTransient<IEmailService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<PostmarkOptions>>().Value;
            return new PostmarkService(options.ApiKey);
        });
        if (configureOptions is { })
        {
            services.Configure(nameof(EmailOptions), configureOptions);
        }
        else
        {
            services
                .AddOptions<EmailOptions>()
                .Bind(configuration.GetSection(nameof(EmailOptions)))
                .ValidateDataAnnotations();
        }

        services.AddScoped<ISecurityContextAccessor, SecurityContextAccessor>();
        var serviceProvider = services.BuildServiceProvider();

        services
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    public static void AddMessageBus(this IServiceCollection services, IConfiguration configuration, Type consumerAssemblyMarkerType)
    {
        services.AddMassTransitWithRabbitMqTransport(configuration, consumerAssemblyMarkerType);

        services.AddScoped<IMessageBus, MessageBus>();
    }
    public static void AddMassTransitWithRabbitMqTransport(
        this IServiceCollection services,
        IConfiguration configuration,
        Type consumerAssemblyMarkerType
    )
    {
        var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

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
                            h.Username(rabbitMqOptions.User);
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
    }
}