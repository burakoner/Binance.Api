using Binance.Api.Spot;

namespace Binance.Api.Converters;

internal class OrderResponseTypeConverter : BaseConverter<BinanceOrderResponseType>
{
    public OrderResponseTypeConverter() : this(true) { }
    public OrderResponseTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceOrderResponseType, string>> Mapping => new List<KeyValuePair<BinanceOrderResponseType, string>>
    {
        new KeyValuePair<BinanceOrderResponseType, string>(BinanceOrderResponseType.Acknowledge, "ACK"),
        new KeyValuePair<BinanceOrderResponseType, string>(BinanceOrderResponseType.Result, "RESULT"),
        new KeyValuePair<BinanceOrderResponseType, string>( BinanceOrderResponseType.Full, "FULL")
    };
}
