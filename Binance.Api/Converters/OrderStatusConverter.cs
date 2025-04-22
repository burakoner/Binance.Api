

namespace Binance.Api.Converters;

internal class OrderStatusConverter : BaseConverter<BinanceOrderStatus>
{
    public OrderStatusConverter() : this(true) { }
    public OrderStatusConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceOrderStatus, string>> Mapping => new List<KeyValuePair<BinanceOrderStatus, string>>
    {
        new KeyValuePair<BinanceOrderStatus, string>(BinanceOrderStatus.New, "NEW"),
        new KeyValuePair<BinanceOrderStatus, string>(BinanceOrderStatus.PartiallyFilled, "PARTIALLY_FILLED"),
        new KeyValuePair<BinanceOrderStatus, string>(BinanceOrderStatus.Filled, "FILLED" ),
        new KeyValuePair<BinanceOrderStatus, string>(BinanceOrderStatus.Canceled, "CANCELED"),
        new KeyValuePair<BinanceOrderStatus, string>(BinanceOrderStatus.PendingCancel, "PENDING_CANCEL"),
        new KeyValuePair<BinanceOrderStatus, string>(BinanceOrderStatus.Rejected, "REJECTED"),
        new KeyValuePair<BinanceOrderStatus, string>(BinanceOrderStatus.Insurance, "NEW_INSURANCE" ),
        new KeyValuePair<BinanceOrderStatus, string>(BinanceOrderStatus.Adl, "NEW_ADL" ),
        new KeyValuePair<BinanceOrderStatus, string>(BinanceOrderStatus.Expired, "EXPIRED" ),
    };
}
