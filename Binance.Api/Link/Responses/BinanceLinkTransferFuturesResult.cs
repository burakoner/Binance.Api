namespace Binance.Api.Link;

/// <summary>
/// Transfer Futures Result
/// </summary>
public record BinanceLinkTransferFuturesResult
{
    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txnId")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Success
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Client Transfer Id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransferId { get; set; } = string.Empty;
}