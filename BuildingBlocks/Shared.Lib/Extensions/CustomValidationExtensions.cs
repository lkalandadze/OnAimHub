using FluentValidation;
using Newtonsoft.Json.Linq;

namespace Shared.Lib.Extensions;

public static class CustomValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeAValidUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(url =>
                Uri.TryCreate(url, UriKind.Absolute, out var result) &&
                (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps));
    }

    public static IRuleBuilderOptions<T, string> MustBeAValidJson<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(content =>
                !string.IsNullOrWhiteSpace(content) &&
                IsJson(content))
            .WithMessage("The value must be a valid JSON string.");
    }

    private static bool IsJson(string content)
    {
        try
        {
            JToken.Parse(content);
            return true;
        }
        catch
        {
            return false;
        }
    }
}