namespace Binance.Api.Futures;

/// <summary>
/// BinanceFuturesStreamUpdate
/// </summary>
public record BinanceFuturesStreamUpdate: BinanceFuturesStreamEvent
{
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;
}
