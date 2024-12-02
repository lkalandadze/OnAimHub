#nullable disable

using Hub.Application.Models.Coin;
using Hub.Domain.Enum;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hub.Application.Converters;

public class CoinModelJsonConverter : JsonConverter<CreateCoinModel>
{
    public override CreateCoinModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        JsonElement? coinTypeProperty = null;

        // Iterate through all properties to find one with a case-insensitive match
        foreach (var property in rootElement.EnumerateObject())
        {
            if (string.Equals(property.Name, nameof(CreateCoinModel.CoinType), StringComparison.OrdinalIgnoreCase))
            {
                coinTypeProperty = property.Value;
                break;
            }
        }

        if (coinTypeProperty == null)
        {
            throw new JsonException("Missing 'coinType' property in a promotion coin.");
        }

        var coinType = (CoinType)coinTypeProperty.Value.GetInt32();

        //if (!rootElement.TryGetProperty("coinType", out var coinTypeProperty))
        //{
        //    throw new JsonException("Missing 'coinType' property in a promotion coin.");
        //}

        //var coinType = (CoinType)coinTypeProperty.GetInt32();
        var json = rootElement.GetRawText();

        return coinType switch
        {
            CoinType.In => JsonSerializer.Deserialize<CreateInCoinModel>(json, options),
            CoinType.Internal => JsonSerializer.Deserialize<CreateInternalCoinModel>(json, options),
            CoinType.Asset => JsonSerializer.Deserialize<CreateAssetCoinModel>(json, options),
            CoinType.Out => JsonSerializer.Deserialize<CreateOutCoinModel>(json, options),
        };
    }

    public override void Write(Utf8JsonWriter writer, CreateCoinModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}