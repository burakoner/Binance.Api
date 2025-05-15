namespace Binance.Api.Broker;

/// <summary>
/// Transfer Result
/// </summary>
public record BinanceBrokerTransferResult
{
    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txnId")]
    public long Id { get; set; }

    /// <summary>
    /// Client Transfer Id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransferId { get; set; } = string.Empty;
}