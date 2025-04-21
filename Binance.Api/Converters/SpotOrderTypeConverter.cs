using Binance.Api.Spot;

namespace Binance.Api.Converters;

internal class SpotOrderTypeConverter : JsonConverter
{
    private readonly bool quotes;

    public SpotOrderTypeConverter()
    {
        quotes = true;
    }

    public SpotOrderTypeConverter(bool useQuotes)
    {
        quotes = useQuotes;
    }

    private readonly Dictionary<BinanceSpotOrderType, string> values = new Dictionary<BinanceSpotOrderType, string>
    {
        { BinanceSpotOrderType.Limit, "LIMIT" },
        { BinanceSpotOrderType.Market, "MARKET" },
        { BinanceSpotOrderType.LimitMaker, "LIMIT_MAKER" },
        { BinanceSpotOrderType.StopLoss, "STOP_LOSS" },
        { BinanceSpotOrderType.StopLossLimit, "STOP_LOSS_LIMIT" },
        { BinanceSpotOrderType.TakeProfit, "TAKE_PROFIT" },
        { BinanceSpotOrderType.TakeProfitLimit, "TAKE_PROFIT_LIMIT" }
    };

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BinanceSpotOrderType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return values.Single(v => v.Value == (string)reader.Value).Key;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (quotes)
            writer.WriteValue(values[(BinanceSpotOrderType)value!]);
        else
            writer.WriteRawValue(values[(BinanceSpotOrderType)value!]);
    }
}
