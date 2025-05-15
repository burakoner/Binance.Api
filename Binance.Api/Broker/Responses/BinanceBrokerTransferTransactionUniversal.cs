namespace Binance.Api.Broker;

/// <summary>
/// Transfer Transaction Universal
/// </summary>
public record BinanceBrokerTransferTransactionUniversal
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
    /// To id
    /// </summary>
    [JsonProperty("toId")]
    public string ToId { get; set; } = string.Empty;

    /// <summary>
    /// From account type
    /// </summary>
    [JsonProperty("fromAccountType")]
    public BinanceBrokerAccountType FromAccountType { get; set; }

    /// <summary>
    /// To account type
    /// </summary>
    [JsonProperty("toAccountType")]
    public BinanceBrokerAccountType ToAccountType { get; set; }

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
    public BinanceBrokerTransferTransactionStatus Status { get; set; }
}