using GameLib.Domain.Entities;
using Microsoft.OpenApi.Writers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameLib.ServiceRegistry
{
    public class GameConfigurationJsonConverter<TGameConfiguration> : JsonConverter<GameConfiguration> where TGameConfiguration : GameConfiguration
    {
        public override GameConfiguration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var rootElement = jsonDocument.RootElement;
            var json = rootElement.GetRawText();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TGameConfiguration>(json)!;
        }

        public override void Write(Utf8JsonWriter writer, GameConfiguration value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}