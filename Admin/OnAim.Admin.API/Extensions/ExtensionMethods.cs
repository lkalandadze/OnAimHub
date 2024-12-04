using OnAim.Admin.Domain.HubEntities.Models;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnAim.Admin.API.Extensions;

public static class ExtensionMethods
{
    public static List<string> GetRoles(this ClaimsPrincipal user)
    {
        return user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
    }
}
public class PromotionCoinModelJsonConverter : JsonConverter<CreateCoinModel>
{
    public override CreateCoinModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        JsonElement? coinTypeProperty = null;

        // Iterate through all properties to find one with a case-insensitive match
        foreach (var property in rootElement.EnumerateObject())
        {
            if (string.Equals(property.Name, "coinType", StringComparison.OrdinalIgnoreCase))
            {
                coinTypeProperty = property.Value;
                break;
            }
        }

        if (coinTypeProperty == null)
        {
            throw new JsonException("Missing 'coinType' property in a promotion coin.");
        }

        var coinType = (Domain.HubEntities.Enum.CoinType)coinTypeProperty.Value.GetInt32();

        //if (!rootElement.TryGetProperty("coinType", out var coinTypeProperty))
        //{
        //    throw new JsonException("Missing 'coinType' property in a promotion coin.");
        //}

        //var coinType = (CoinType)coinTypeProperty.GetInt32();
        var json = rootElement.GetRawText();

        return coinType switch
        {
            Domain.HubEntities.Enum.CoinType.In => JsonSerializer.Deserialize<CreateInCoinModel>(json, options),
            Domain.HubEntities.Enum.CoinType.Internal => JsonSerializer.Deserialize<CreateInternalCoinModel>(json, options),
            Domain.HubEntities.Enum.CoinType.Asset => JsonSerializer.Deserialize<CreateAssetCoinModel>(json, options),
            Domain.HubEntities.Enum.CoinType.Out => JsonSerializer.Deserialize<CreateOutCoinModel>(json, options),
        };
    }

    public override void Write(Utf8JsonWriter writer, CreateCoinModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}