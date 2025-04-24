using Binance.Api.Futures;

namespace Binance.Api.Converters;

internal class FuturesMarginTypeConverter : BaseConverter<BinanceFuturesMarginType>
{
    public FuturesMarginTypeConverter() : this(false) { }
    public FuturesMarginTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceFuturesMarginType, string>> Mapping => new List<KeyValuePair<BinanceFuturesMarginType, string>>
    {
        new KeyValuePair<BinanceFuturesMarginType, string>(BinanceFuturesMarginType.Isolated, "ISOLATED"),
        new KeyValuePair<BinanceFuturesMarginType, string>(BinanceFuturesMarginType.Cross, "CROSSED"),
        new KeyValuePair<BinanceFuturesMarginType, string>(BinanceFuturesMarginType.Cross, "cross") //return on BinanceFuturesStreamPosition
    };
}
