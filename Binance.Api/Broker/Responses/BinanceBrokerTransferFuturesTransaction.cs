using Binance.Api.SubAccount;

namespace Binance.Api.Broker;

/// <summary>
/// Transfer Futures Transactions
/// </summary>
public record BinanceBrokerageTransferFuturesTransactions
{
    /// <summary>
    /// Success
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Futures type
    /// </summary>
    [JsonProperty("futuresType")]
    public BinanceFuturesType FuturesType { get; set; }

    /// <summary>
    /// Transfer
    /// </summary>
    [JsonProperty("transfer")]
    public List<BinanceBrokerTransferFuturesTransaction> Transactions { get; set; } = [];
}

/// <summary>
/// Transfer Futures Transaction
/// </summary>
public record BinanceBrokerTransferFuturesTransaction
{
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
    /// Transaction Id
    /// </summary>
    [JsonProperty("tranId")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Client Transfer Id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransferId { get; set; } = string.Empty;

    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
}