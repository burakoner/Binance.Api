namespace Binance.Api.Link;

/// <summary>
/// Transfer Transaction
/// </summary>
public record BinanceLinkTransferTransaction
{
    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txnId")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Client Transfer Id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransferId { get; set; } = string.Empty;

    /// <summary>
    /// From Id
    /// </summary>
    [JsonProperty("fromId")]
    public string FromId { get; set; } = string.Empty;

    /// <summary>
    /// To Id
    /// </summary>
    [JsonProperty("toId")]
    public string ToId { get; set; } = string.Empty;

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceLinkTransferTransactionStatus Status { get; set; }
}