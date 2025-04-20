namespace Binance.Api.Models.WebSocketApi.MarketData;

/// <summary>
/// Order list info
/// </summary>
public record BinanceStreamOrderList : BinanceSocketEvent
{
    /// <summary>
    /// The id of the order list
    /// </summary>
    [JsonProperty("g")]
    public long Id { get; set; }
    /// <summary>
    /// The contingency type
    /// </summary>
    [JsonProperty("c")]
    public string ContingencyType { get; set; } = "";
    /// <summary>
    /// The order list status
    /// </summary>
    [JsonConverter(typeof(ListStatusTypeConverter))]
    [JsonProperty("l")]
    public ListStatusType ListStatusType { get; set; }
    /// <summary>
    /// The order status
    /// </summary>
    [JsonConverter(typeof(ListOrderStatusConverter))]
    [JsonProperty("L")]
    public ListOrderStatus ListOrderStatus { get; set; }
    /// <summary>
    /// Rejection reason
    /// </summary>
    [JsonProperty("r")]
    public string ListRejectReason { get; set; } = "";
    /// <summary>
    /// The client id of the order list
    /// </summary>
    [JsonProperty("C")]
    public string ListClientOrderId { get; set; } = "";
    /// <summary>
    /// The transaction time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("T")]
    public DateTime TransactionTime { get; set; }
    /// <summary>
    /// The symbol of the order list
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";
    /// <summary>
    /// The order in this list
    /// </summary>
    [JsonProperty("O")]
    public IEnumerable<BinanceStreamOrderId> Orders { get; set; } = [];
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    public string ListenKey { get; set; } = "";
}

/// <summary>
/// Order reference
/// </summary>
public record BinanceStreamOrderId
{
    /// <summary>
    /// The symbol of the order
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";
    /// <summary>
    /// The id of the order
    /// </summary>
    [JsonProperty("i")]
    public long OrderId { get; set; }
    /// <summary>
    /// The client order id
    /// </summary>
    [JsonProperty("c")]
    public string ClientOrderId { get; set; } = "";
}
