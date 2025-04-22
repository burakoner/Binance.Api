

namespace Binance.Api.Converters;

internal class PositionSideConverter : BaseConverter<BinancePositionSide>
{
    public PositionSideConverter() : this(true) { }
    public PositionSideConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinancePositionSide, string>> Mapping => new List<KeyValuePair<BinancePositionSide, string>>
    {
        new KeyValuePair<BinancePositionSide, string>(BinancePositionSide.Short, "SHORT"),
        new KeyValuePair<BinancePositionSide, string>(BinancePositionSide.Long, "LONG"),
        new KeyValuePair<BinancePositionSide, string>(BinancePositionSide.Both, "BOTH"),
    };
}
