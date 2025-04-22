using Binance.Api.Spot;

namespace Binance.Api.Shared;

/// <summary>
/// The result of placing a new OCO order
/// </summary>
public record BinanceOrderOcoList
{
    /// <summary>
    /// The id of the order list
    /// </summary>
    [JsonProperty("orderListId")]
    public long Id { get; set; }

    /// <summary>
    /// The contingency type
    /// </summary>
    public string ContingencyType { get; set; } = "";

    /// <summary>
    /// The order list status
    /// </summary>
    [JsonConverter(typeof(ListStatusTypeConverter))]
    public BinanceListStatusType ListStatusType { get; set; }

    /// <summary>
    /// The order status
    /// </summary>
    [JsonConverter(typeof(ListOrderStatusConverter))]
    public BinanceListOrderStatus ListOrderStatus { get; set; }

    /// <summary>
    /// The client id of the order list
    /// </summary>
    [JsonProperty("listClientOrderId")]
    [JsonConverterCtor(typeof(ReplaceConverter),
        $"{BinanceExchange.ClientOrderIdPrefixSpot}->",
        $"{BinanceExchange.ClientOrderIdPrefixFutures}->")]
    public string ListClientOrderId { get; set; } = string.Empty;

    /// <summary>
    /// The transaction time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The symbol of the order list
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The order in this list
    /// </summary>
    public IEnumerable<BinanceOrderId> Orders { get; set; } = [];

    /// <summary>
    /// The order details
    /// </summary>
    public IEnumerable<BinancePlacedOcoOrder> OrderReports { get; set; } = [];
}

/// <summary>
/// Order reference
/// </summary>
public record BinanceOrderId
{
    /// <summary>
    /// The symbol of the order
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The id of the order
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// The client order id
    /// </summary>
    [JsonPropertyName("clientOrderId")]
    [JsonConverterCtor(typeof(ReplaceConverter),
        $"{BinanceExchange.ClientOrderIdPrefixSpot}->",
        $"{BinanceExchange.ClientOrderIdPrefixFutures}->")]
    public string ClientOrderId { get; set; } = string.Empty;
}

/// <summary>
/// The result of placing a new order
/// </summary>
public record BinancePlacedOcoOrder : BinanceOrderBase
{
}
