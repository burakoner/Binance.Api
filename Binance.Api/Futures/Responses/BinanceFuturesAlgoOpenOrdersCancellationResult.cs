namespace Binance.Api.Futures;

/// <summary>
/// Native USD-M open conditional Algo orders cancellation result
/// </summary>
public record BinanceFuturesAlgoOpenOrdersCancellationResult
{
    /// <summary>
    /// Result code
    /// </summary>
    [JsonProperty("code")]
    public long Code { get; set; }

    /// <summary>
    /// Result message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;
}
