namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Transfer Transaction
/// </summary>
public record BinanceBrokerageTransferTransaction
{
    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txnId")]
    public string Id { get; set; } = "";

    /// <summary>
    /// Client Transfer Id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransferId { get; set; } = "";

    /// <summary>
    /// From Id
    /// </summary>
    public string FromId { get; set; } = "";

    /// <summary>
    /// To Id
    /// </summary>
    public string ToId { get; set; } = "";

    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = "";

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
    [JsonConverter(typeof(BrokerageTransferTransactionStatusConverter))]
    public BrokerageTransferTransactionStatus Status { get; set; }
}