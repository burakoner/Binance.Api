namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Transfer Futures Transactions
/// </summary>
public record BinanceBrokerageTransferFuturesTransactions
{
    /// <summary>
    /// Success
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Futures type
    /// </summary>
    public BinanceBrokerageFuturesType FuturesType { get; set; }

    /// <summary>
    /// Transfer
    /// </summary>
    [JsonProperty("transfer")]
    public IEnumerable<BinanceBrokerageTransferFuturesTransaction> Transactions { get; set; } = Array.Empty<BinanceBrokerageTransferFuturesTransaction>();
}

/// <summary>
/// Transfer Futures Transaction
/// </summary>
public record BinanceBrokerageTransferFuturesTransaction
{
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
    /// Transaction Id
    /// </summary>
    [JsonProperty("tranId")]
    public string Id { get; set; } = "";

    /// <summary>
    /// Client Transfer Id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransferId { get; set; } = "";

    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
}