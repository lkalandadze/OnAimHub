using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnAim.Admin.APP.Factory;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Infrasturcture.Repository;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.Shared.Models;
using OnAim.Admin.Shared.Csv;
using OnAim.Admin.Shared.HtmlGenerators;
using OnAim.Admin.Shared.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using OnAim.Admin.APP.Email;


namespace OnAim.Admin.APP
{
    public static class Extension
    {
        public static IServiceCollection AddApp(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<EmailOptions>? configureOptions = null)
        {
            var emailOptions = new EmailOptions();
            configuration.GetSection(nameof(EmailOptions)).Bind(emailOptions);
            configureOptions?.Invoke(emailOptions);
            services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));

            services
                .AddScoped<IEndpointRepository, EndpointRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IPermissionService, PermissionService>()
                .AddTransient<IJwtFactory, JwtFactory>();
            services.AddHostedService<TokenCleanupService>();
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IRejectedLogRepository, RejectedLogRepository>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IDomainValidationService, DomainValidationService>();
            services.AddScoped<IAppSettingsService, AppSettingsService>();
            services.AddScoped(typeof(ICsvWriter<>), typeof(CsvWriter<>));
            services.AddHtmlGenerator();

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
            //services.AddHostedService<RejectedLogRetryService>();

            services.AddScoped<ISecurityContextAccessor, SecurityContextAccessor>();

            var serviceProvider = services.BuildServiceProvider();

            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> src)
        => src ?? Enumerable.Empty<T>();
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
}
