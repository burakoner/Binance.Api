namespace Binance.Api.Converters;

internal class ListStatusTypeConverter : BaseConverter<BinanceListStatusType>
{
    public ListStatusTypeConverter() : this(true) { }
    public ListStatusTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceListStatusType, string>> Mapping => new List<KeyValuePair<BinanceListStatusType, string>>
    {
        new KeyValuePair<BinanceListStatusType, string>(BinanceListStatusType.Response, "RESPONSE"),
        new KeyValuePair<BinanceListStatusType, string>(BinanceListStatusType.ExecutionStarted, "EXEC_STARTED"),
        new KeyValuePair<BinanceListStatusType, string>(BinanceListStatusType.Done, "ALL_DONE"),
    };
}
