namespace Binance.Api.Shared;

internal sealed class BinanceNullableBooleanConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
        => objectType == typeof(bool) || objectType == typeof(bool?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        if (reader.TokenType == JsonToken.Boolean)
            return reader.Value;

        if (reader.TokenType == JsonToken.String)
        {
            var value = (string?)reader.Value;
            if (string.IsNullOrWhiteSpace(value) || string.Equals(value, "null", StringComparison.OrdinalIgnoreCase))
                return null;
            if (bool.TryParse(value, out var result))
                return result;
        }

        throw new JsonSerializationException($"Unexpected nullable boolean value '{reader.Value}'");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        => writer.WriteValue(value);
}
