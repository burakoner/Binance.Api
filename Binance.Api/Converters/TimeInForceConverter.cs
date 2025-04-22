using Binance.Api.Spot;

namespace Binance.Api.Converters;

internal class TimeInForceConverter : BaseConverter<BinanceTimeInForce>
{
    public TimeInForceConverter() : this(true) { }
    public TimeInForceConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceTimeInForce, string>> Mapping => new List<KeyValuePair<BinanceTimeInForce, string>>
    {
        new KeyValuePair<BinanceTimeInForce, string>(BinanceTimeInForce.GoodTillCanceled, "GTC"),
        new KeyValuePair<BinanceTimeInForce, string>(BinanceTimeInForce.ImmediateOrCancel, "IOC"),
        new KeyValuePair<BinanceTimeInForce, string>(BinanceTimeInForce.FillOrKill, "FOK"),
        new KeyValuePair<BinanceTimeInForce, string>(BinanceTimeInForce.GoodTillCrossing, "GTX"),
        new KeyValuePair<BinanceTimeInForce, string>(BinanceTimeInForce.GoodTillExpiredOrCanceled, "GTE_GTC")
    };
}
