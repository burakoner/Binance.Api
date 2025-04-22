using Binance.Api.Spot;

namespace Binance.Api.Converters;

internal class OrderSideConverter : BaseConverter<BinanceOrderSide>
{
    public OrderSideConverter() : this(true) { }
    public OrderSideConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceOrderSide, string>> Mapping => new List<KeyValuePair<BinanceOrderSide, string>>
    {
        new KeyValuePair<BinanceOrderSide, string>(BinanceOrderSide.Buy, "BUY"),
        new KeyValuePair<BinanceOrderSide, string>(BinanceOrderSide.Sell, "SELL")
    };
}
