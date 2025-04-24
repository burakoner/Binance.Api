using Binance.Api.Futures;

namespace Binance.Api.Converters;

internal class FuturesOrderTypeConverter : JsonConverter
{
    private readonly bool quotes;

    public FuturesOrderTypeConverter()
    {
        quotes = true;
    }

    public FuturesOrderTypeConverter(bool useQuotes)
    {
        quotes = useQuotes;
    }

    private readonly Dictionary<BinanceFuturesOrderType, string> values = new Dictionary<BinanceFuturesOrderType, string>
    {
        { BinanceFuturesOrderType.Limit, "LIMIT" },
        { BinanceFuturesOrderType.Market, "MARKET" },
        { BinanceFuturesOrderType.TakeProfit, "TAKE_PROFIT" },
        { BinanceFuturesOrderType.TakeProfitMarket, "TAKE_PROFIT_MARKET" },
        { BinanceFuturesOrderType.Stop, "STOP" },
        { BinanceFuturesOrderType.StopMarket, "STOP_MARKET" },
        { BinanceFuturesOrderType.TrailingStopMarket, "TRAILING_STOP_MARKET" },
        { BinanceFuturesOrderType.Liquidation, "LIQUIDATION" }
    };

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BinanceFuturesOrderType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return values.Single(v => v.Value == (string)reader.Value).Key;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (quotes)
            writer.WriteValue(values[(BinanceFuturesOrderType)value!]);
        else
            writer.WriteRawValue(values[(BinanceFuturesOrderType)value!]);
    }
}
