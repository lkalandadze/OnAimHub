using Microsoft.Extensions.DependencyInjection;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Repository;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using OnAim.Admin.Contracts.Models;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using OnAim.Admin.APP.Services.Email;
using OnAim.Admin.APP.Feature.Identity;
using OnAim.Admin.Contracts.Helpers.Csv;
using OnAim.Admin.Contracts.ApplicationInfrastructure.Configuration;
using OnAim.Admin.APP.Services.AuthServices;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Repositories;
using OnAim.Admin.APP.Services.SettingServices;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OnAim.Admin.APP.Services.ClientService;
using Autofac;
using System.Reflection;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.APP.Services.Domain;
using OnAim.Admin.APP.Services.User;
using OnAim.Admin.APP.Services.Endpoint;
using OnAim.Admin.APP.Services.EndpointGroup;
using OnAim.Admin.APP.Services.Role;
using OnAim.Admin.APP.Services.Segment;
using OnAim.Admin.APP.Services.Player;
using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using ValidationException = OnAim.Admin.CrossCuttingConcerns.Exceptions.ValidationException;
using Shared.Infrastructure.Bus;
using Shared.Infrastructure.MassTransit;
using MassTransit;
using OnAim.Admin.APP.Services.LeaderBoard;
using OnAim.Admin.APP.Services.ClientServices;


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

        services
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<ILogRepository, LogRepository>()
            .AddScoped<IPermissionService, PermissionService>()
            .AddScoped<IPasswordService, PasswordService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IEndpointService, EndpointService>()
            .AddScoped<IEndpointGroupService, EndpointGroupService>()
            .AddScoped<IRoleService, RoleService>()
            .AddScoped<IDomainService, DomainService>()
            .AddScoped<IGameService, GameService>()
            .AddScoped<ISegmentService, SegmentService>()
            .AddScoped<ILeaderBoardService, LeaderBoardService>()
            .AddScoped<IPlayerService, PlayerService>()
            .AddTransient<IJwtFactory, JwtFactory>()
            .AddHostedService<TokenCleanupService>()
            .Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"))
            .AddScoped<IDomainValidationService, DomainValidationService>()
            .AddScoped<IAppSettingsService, AppSettingsService>()
            .AddScoped<IOtpService, OtpService>()
            .AddScoped(typeof(ICsvWriter<>), typeof(CsvWriter<>))
            .AddScoped(typeof(CommandContext<>), typeof(CommandContext<>));
        services
            .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IConfigurationRepository<>), typeof(ConfigurationRepository<>))
            .AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>))
            .AddScoped(typeof(ILeaderBoardReadOnlyRepository<>), typeof(LeaderBoardReadOnlyRepository<>));
        services.AddHttpClient("ApiGateway", client =>
        {
            client.BaseAddress = new Uri("http://ocelotapigateway:8080");
        });
        services.Configure<PostmarkOptions>(configuration.GetSection("Postmark"));

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
        services.AddMessageBus(configuration, consumerAssemblyMarkerType);
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
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
public static partial class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddCustomHttpClients(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatedOptions<PolicyOptions>();

        AddHubApiClient(builder);

        return builder;
    }

    private static void AddHubApiClient(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatedOptions<HubApiClientOptions>();

        builder.Services.AddValidatedOptions<LeaderBoardApiClientOptions>();

        builder.Services.AddHttpClient<IHubApiClient, HubApiClient>(
            (client, sp) =>
            {
                var catalogApiOptions = sp.GetRequiredService<IOptions<HubApiClientOptions>>();
                var policyOptions = sp.GetRequiredService<IOptions<PolicyOptions>>();
                catalogApiOptions.Value.NotBeNull();

                var baseAddress = catalogApiOptions.Value.BaseApiAddress;
                client.BaseAddress = new Uri(baseAddress);
                return new HubApiClient(client, catalogApiOptions, policyOptions, "admin", "password");
            }
        );

        builder.Services.AddHttpClient<ILeaderBoardApiClient, HttpClientService>(
           (client, sp) =>
           {
               var catalogApiOptions = sp.GetRequiredService<IOptions<LeaderBoardApiClientOptions>>();
               var policyOptions = sp.GetRequiredService<IOptions<PolicyOptions>>();
               catalogApiOptions.Value.NotBeNull();

               var baseAddress = catalogApiOptions.Value.BaseApiAddress;
               client.BaseAddress = new Uri(baseAddress);
               return new HttpClientService(client, catalogApiOptions, policyOptions);
           }
       );
    }

    public static IServiceCollection AddValidatedOptions<T>(
        this IServiceCollection services,
        string? key = null,
        Func<T, bool>? validator = null,
        Action<T>? configurator = null
    )
        where T : class
    {
        validator ??= RequiredConfigurationValidator.Validate;

        var optionBuilder = services.AddOptions<T>().BindConfiguration(key ?? typeof(T).Name);

        if (configurator is not null)
        {
            optionBuilder = optionBuilder.Configure(configurator);
        }

        optionBuilder.Validate(validator);

        services.TryAddSingleton(x => x.GetRequiredService<IOptions<T>>().Value);

        return services;
    }
    public static class RequiredConfigurationValidator
    {
        public static bool Validate<T>(T arg)
            where T : class
        {
            var requiredProperties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => Attribute.IsDefined(x, typeof(RequiredMemberAttribute)));

            foreach (var requiredProperty in requiredProperties)
            {
                var propertyValue = requiredProperty.GetValue(arg);
                if (propertyValue is null)
                {
                    throw new System.Exception($"Required property '{requiredProperty.Name}' was null");
                }
            }

            return true;
        }
    }
}

public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessStatusCodeWithDetailAsync(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = await response.Content.ReadAsStringAsync();

        throw new HttpResponseException(content, (int)response.StatusCode);
    }
}

public static class ValidationExtensions
{

    public static TRequest HandleValidation<TRequest>(this IValidator<TRequest> validator, TRequest request)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors[0].ErrorMessage);

        return request;
    }
    public static T NotBeNull<T>(
        [NotNull] this T? argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument == null)
        {
            throw new ValidationException(message: $"{argumentName} cannot be null or empty.");
        }

        return argument;
    }

    public static T NotBeNull<T>([NotNull] this T? argument, System.Exception exception)
    {
        if (argument == null)
        {
            throw exception;
        }

        return argument;
    }

    public static string NotBeEmpty(
        this string argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument.Length == 0)
        {
            throw new ValidationException($"{argumentName} cannot be null or empty.");
        }

        return argument;
    }

    public static string NotBeEmptyOrNull(
        [NotNull] this string? argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (string.IsNullOrEmpty(argument))
        {
            throw new ValidationException($"{argumentName} cannot be null or empty.");
        }

        return argument;
    }

    public static string NotBeNullOrWhiteSpace(
        [NotNull] this string? argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ValidationException($"{argumentName} cannot be null or white space.");
        }

        return argument;
    }

    public static Guid NotBeEmpty(
        this Guid argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument == Guid.Empty)
        {
            throw new ValidationException($"{argumentName} cannot be empty.");
        }

        return argument;
    }

    public static Guid NotBeEmpty(
        [NotNull] this Guid? argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument is null)
        {
            throw new ValidationException(message: $"{argumentName} cannot be null or empty.");
        }

        return argument.Value.NotBeEmpty();
    }

    public static int NotBeNegativeOrZero(
        this int argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument == 0)
        {
            throw new ValidationException($"{argumentName} cannot be zero.");
        }

        return argument;
    }

    public static long NotBeNegativeOrZero(
        [NotNull] this long? argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument is null)
        {
            throw new ValidationException(message: $"{argumentName} cannot be null or empty.");
        }

        return argument.Value.NotBeNegativeOrZero();
    }

    public static long NotBeNegativeOrZero(
        this long argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument == 0)
        {
            throw new ValidationException($"{argumentName} cannot be zero.");
        }

        return argument;
    }

    public static long NotBeNegativeOrZero(
        [NotNull] this int? argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument is null)
        {
            throw new ValidationException(message: $"{argumentName} cannot be null or empty.");
        }

        return argument.Value.NotBeNegativeOrZero();
    }

    public static decimal NotBeNegativeOrZero(
        this decimal argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument == 0)
        {
            throw new ValidationException($"{argumentName} cannot be zero.");
        }

        return argument;
    }

    public static decimal NotBeNegativeOrZero(
        [NotNull] this decimal? argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument is null)
        {
            throw new ValidationException(message: $"{argumentName} cannot be null or empty.");
        }

        return argument.Value.NotBeNegativeOrZero();
    }

    public static double NotBeNegativeOrZero(
        this double argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument == 0)
        {
            throw new ValidationException($"{argumentName} cannot be zero.");
        }

        return argument;
    }

    public static double NotBeNegativeOrZero(
        [NotNull] this double? argument,
        [CallerArgumentExpression("argument")] string? argumentName = null
    )
    {
        if (argument is null)
        {
            throw new ValidationException(message: $"{argumentName} cannot be null or empty.");
        }

        return argument.Value.NotBeNegativeOrZero();
    }

    public static string NotBeInvalidEmail(
        this string email,
        [CallerArgumentExpression("email")] string? argumentName = null
    )
    {
        // Use Regex to validate email format
        var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        if (!regex.IsMatch(email))
        {
            throw new ValidationException($"{argumentName} is not a valid email address.");
        }

        return email;
    }

    public static string NotBeInvalidPhoneNumber(
        this string phoneNumber,
        [CallerArgumentExpression("phoneNumber")] string? argumentName = null
    )
    {
        // Use Regex to validate phone number format
        // valid phones: +10---------- , (+10)----------
        var regex = new Regex(@"^[+]?[(]?[+]?[0-9]{1,4}[)]?[-\s./0-9]{9,12}$");

        if (!regex.IsMatch(phoneNumber))
        {
            throw new ValidationException($"{argumentName} is not a valid phone number.");
        }

        return phoneNumber;
    }

    public static string NotBeInvalidMobileNumber(
        this string mobileNumber,
        [CallerArgumentExpression("mobileNumber")] string? argumentName = null
    )
    {
        // Use Regex to validate mobile number format
        var regex = new Regex(@"^(?:\+|00)?(\(\d{1,3}\)|\d{1,3})?([1-9]\d{9})$");

        if (!regex.IsMatch(mobileNumber))
        {
            throw new ValidationException($"{argumentName} is not a valid mobile number.");
        }

        return mobileNumber;
    }

    public static TEnum NotBeEmptyOrNull<TEnum>(
        [NotNull] this TEnum? enumValue,
        [CallerArgumentExpression("enumValue")] string argumentName = ""
    )
        where TEnum : Enum
    {
        if (enumValue is null)
        {
            throw new ValidationException(message: $"{argumentName} cannot be null or empty.");
        }

        enumValue.NotBeEmpty();

        return enumValue;
    }

    public static TEnum NotBeEmpty<TEnum>(
        [NotNull] this TEnum enumValue,
        [CallerArgumentExpression("enumValue")] string? argumentName = null
    )
        where TEnum : Enum
    {
        enumValue.NotBeNull();
        if (enumValue.Equals(default(TEnum)))
        {
            throw new ValidationException(
                $"The value of '{argumentName}' cannot be the default value of '{typeof(TEnum).Name}' enum."
            );
        }

        return enumValue;
    }

    public static void NotBeEmpty(
        this DateTime dateTime,
        [CallerArgumentExpression("dateTime")] string? argumentName = null
    )
    {
        var isEmpty = dateTime == DateTime.MinValue;
        if (isEmpty)
        {
            throw new ValidationException(
                $"The value of '{argumentName}' cannot be the default value of '{dateTime}'."
            );
        }
    }

}