namespace Binance.Api.Futures;

/// <summary>
/// Native USD-M conditional Algo order cancellation result
/// </summary>
public record BinanceFuturesAlgoOrderCancellationResult
{
    /// <summary>
    /// Exchange-assigned Algo order ID
    /// </summary>
    [JsonProperty("algoId")]
    public long AlgoId { get; set; }

    /// <summary>
    /// Client-assigned Algo order ID
    /// </summary>
    [JsonProperty("clientAlgoId")]
    public string ClientAlgoId { get; set; } = string.Empty;

    /// <summary>
    /// Result code as published by Binance
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Result message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;
}
