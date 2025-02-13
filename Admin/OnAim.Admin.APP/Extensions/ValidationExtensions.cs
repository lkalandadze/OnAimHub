﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ValidationException = OnAim.Admin.CrossCuttingConcerns.Exceptions.ValidationException;

namespace OnAim.Admin.APP.Extensions;

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