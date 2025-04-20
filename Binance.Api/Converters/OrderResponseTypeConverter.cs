namespace Binance.Api.Converters;

internal class OrderResponseTypeConverter : BaseConverter<BinanceSpotOrderResponseType>
{
    public OrderResponseTypeConverter() : this(true) { }
    public OrderResponseTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceSpotOrderResponseType, string>> Mapping => new List<KeyValuePair<BinanceSpotOrderResponseType, string>>
    {
        new KeyValuePair<BinanceSpotOrderResponseType, string>(BinanceSpotOrderResponseType.Acknowledge, "ACK"),
        new KeyValuePair<BinanceSpotOrderResponseType, string>(BinanceSpotOrderResponseType.Result, "RESULT"),
        new KeyValuePair<BinanceSpotOrderResponseType, string>( BinanceSpotOrderResponseType.Full, "FULL")
    };
}
