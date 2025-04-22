namespace Binance.Api.Converters;

internal class ListOrderStatusConverter : BaseConverter<BinanceListOrderStatus>
{
    public ListOrderStatusConverter() : this(true) { }
    public ListOrderStatusConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceListOrderStatus, string>> Mapping => new List<KeyValuePair<BinanceListOrderStatus, string>>
    {
        new KeyValuePair<BinanceListOrderStatus, string>(BinanceListOrderStatus.Executing, "EXECUTING"),
        new KeyValuePair<BinanceListOrderStatus, string>(BinanceListOrderStatus.Rejected, "REJECT"),
        new KeyValuePair<BinanceListOrderStatus, string>(BinanceListOrderStatus.Done, "ALL_DONE"),
    };
}
