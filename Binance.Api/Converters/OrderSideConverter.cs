namespace Binance.Api.Converters;

internal class OrderSideConverter : BaseConverter<BinanceSpotOrderSide>
{
    public OrderSideConverter() : this(true) { }
    public OrderSideConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceSpotOrderSide, string>> Mapping => new List<KeyValuePair<BinanceSpotOrderSide, string>>
    {
        new KeyValuePair<BinanceSpotOrderSide, string>(BinanceSpotOrderSide.Buy, "BUY"),
        new KeyValuePair<BinanceSpotOrderSide, string>(BinanceSpotOrderSide.Sell, "SELL")
    };
}
