namespace Binance.Api.Spot;

/// <summary>
/// Order list info
/// </summary>
public record BinanceSpotStreamOrderListUpdate: BinanceSpotStreamEvent
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
    public string ContingencyType { get; set; } = string.Empty;

    /// <summary>
    /// The order list status
    /// </summary>
    [JsonProperty("l")]
    public BinanceListStatusType ListStatusType { get; set; }

    /// <summary>
    /// The order status
    /// </summary>
    [JsonProperty("L")]
    public BinanceListOrderStatus ListOrderStatus { get; set; }

    /// <summary>
    /// Rejection reason
    /// </summary>
    [JsonProperty("r")]
    public string? ListRejectReason { get; set; }

    /// <summary>
    /// The client id of the order list
    /// </summary>
    [JsonProperty("C")]
    public string ListClientOrderId { get; set; } = string.Empty;

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
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The order in this list
    /// </summary>
    [JsonProperty("O")]
    public List<BinanceSpotStreamOrderId> Orders { get; set; } = [];

    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;
}
