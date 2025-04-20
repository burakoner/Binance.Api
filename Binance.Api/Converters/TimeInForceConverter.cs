namespace Binance.Api.Converters;

internal class TimeInForceConverter : BaseConverter<BinanceSpotTimeInForce>
{
    public TimeInForceConverter() : this(true) { }
    public TimeInForceConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceSpotTimeInForce, string>> Mapping => new List<KeyValuePair<BinanceSpotTimeInForce, string>>
    {
        new KeyValuePair<BinanceSpotTimeInForce, string>(BinanceSpotTimeInForce.GoodTillCanceled, "GTC"),
        new KeyValuePair<BinanceSpotTimeInForce, string>(BinanceSpotTimeInForce.ImmediateOrCancel, "IOC"),
        new KeyValuePair<BinanceSpotTimeInForce, string>(BinanceSpotTimeInForce.FillOrKill, "FOK"),
        new KeyValuePair<BinanceSpotTimeInForce, string>(BinanceSpotTimeInForce.GoodTillCrossing, "GTX"),
        new KeyValuePair<BinanceSpotTimeInForce, string>(BinanceSpotTimeInForce.GoodTillExpiredOrCanceled, "GTE_GTC")
    };
}
