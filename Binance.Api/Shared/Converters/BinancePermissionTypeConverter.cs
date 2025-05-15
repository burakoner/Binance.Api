namespace Binance.Api.Shared;

internal class BinancePermissionTypeConverter : JsonConverter
{
    public override bool CanConvert(Type type)
    {
        return type == typeof(BinancePermissionType)
            || type == typeof(List<BinancePermissionType>)
            || type == typeof(List<List<BinancePermissionType>>);
    }

    public override object? ReadJson(JsonReader reader, Type type, object? existingValue, JsonSerializer serializer)
    {
        if (type == typeof(BinancePermissionType))
        {
            return ParseBinancePermissionType(ref reader) ?? null;
        }
        else if (type == typeof(List<BinancePermissionType>))
        {
            var result = new List<BinancePermissionType>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartArray)
                    continue;

                var parseResult = ParseBinancePermissionType(ref reader);
                if (parseResult != null)
                    result.Add(parseResult.Value);
            }
            return result;
        }
        else if (type == typeof(List<List<BinancePermissionType>>))
        {
            var result = new List<List<BinancePermissionType>>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                var resultInner = new List<BinancePermissionType>();
                while (true)
                {
                    if (reader.TokenType == JsonToken.EndArray)
                        break;

                    var parseResult = ParseBinancePermissionType(ref reader);
                    if (parseResult != null) resultInner.Add(parseResult.Value);
                }
                var resultInnerDistinct = resultInner.Distinct().ToList();
                result.Add(resultInnerDistinct);
            }

            return result;
        }

        throw new InvalidOperationException("Invalid type");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is BinancePermissionType act)
        {
            WriteBinancePermissionType(writer, act);
        }
        else if (value is List<BinancePermissionType> actList)
        {
            writer.WriteStartArray();
            foreach (var val in actList)
                WriteBinancePermissionType(writer, val);
            writer.WriteEndArray();
        }
        else if (value is List<List<BinancePermissionType>> actListList)
        {
            writer.WriteStartArray();
            foreach (var valList in actListList)
            {
                writer.WriteStartArray();
                foreach (var val in valList)
                    WriteBinancePermissionType(writer, val);
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
    }

    private void WriteBinancePermissionType(JsonWriter writer, BinancePermissionType value)
    {
        if (value == BinancePermissionType.Spot)
            writer.WriteValue("SPOT");
        if (value == BinancePermissionType.Margin)
            writer.WriteValue("MARGIN");
        if (value == BinancePermissionType.Futures)
            writer.WriteValue("FUTURES");
        if (value == BinancePermissionType.Leveraged)
            writer.WriteValue("LEVERAGED");
        if (value == BinancePermissionType.PreMarket)
            writer.WriteValue("PRE_MARKET");

        writer.WriteValue("TRD_GRP_001");
    }

    private BinancePermissionType? ParseBinancePermissionType(ref JsonReader reader)
    {
        var str = reader.ReadAsString();
        if (string.IsNullOrEmpty(str)) return null;

        if (str!.StartsWith("TRD_GRP_"))
        {
            var number = str.Substring(8);
            if (Enum.TryParse<BinancePermissionType>("TradeGroup" + number, out var value))
                return value;

            return null;
        }
        else
        {
            if (str.Equals("SPOT", StringComparison.Ordinal))
                return BinancePermissionType.Spot;
            if (str.Equals("MARGIN", StringComparison.Ordinal))
                return BinancePermissionType.Margin;
            if (str.Equals("FUTURES", StringComparison.Ordinal))
                return BinancePermissionType.Futures;
            if (str.Equals("LEVERAGED", StringComparison.Ordinal))
                return BinancePermissionType.Leveraged;
            if (str.Equals("PRE_MARKET", StringComparison.Ordinal))
                return BinancePermissionType.PreMarket;
        }

        return null;
    }
}
