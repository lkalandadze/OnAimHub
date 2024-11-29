#nullable disable

using Hub.Application.Models.PromotionCoin;
using Hub.Domain.Enum;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hub.Application.Converters;

public class PromotionCoinModelJsonConverter : JsonConverter<BaseCreatePromotionCoinModel>
{
    public override BaseCreatePromotionCoinModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        if (!rootElement.TryGetProperty("coinType", out var coinTypeProperty))
        {
            throw new JsonException("Missing 'coinType' property in a promotion coin.");
        }

        var coinType = (CoinType)coinTypeProperty.GetInt32();
        var json = rootElement.GetRawText();

        return coinType switch
        {
            CoinType.Incomming => JsonSerializer.Deserialize<CreatePromotionIncomingCoinModel>(json, options),
            CoinType.Internal => JsonSerializer.Deserialize<CreatePromotionInternalCoinModel>(json, options),
            CoinType.Prize => JsonSerializer.Deserialize<CreatePromotionPrizeCoinModel>(json, options),
            CoinType.Outgoing => JsonSerializer.Deserialize<CreatePromotionOutgoingCoinModel>(json, options),
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseCreatePromotionCoinModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}