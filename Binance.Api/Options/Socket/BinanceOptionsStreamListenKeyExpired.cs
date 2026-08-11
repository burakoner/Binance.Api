namespace Binance.Api.Options;

/// <summary>
/// Options listen-key expiration event
/// </summary>
public record BinanceOptionsStreamListenKeyExpired : BinanceSocketStreamEvent
{
    /// <summary>
    /// Expired listen key
    /// </summary>
    [JsonProperty("listenKey")]
    public string ListenKey { get; set; } = string.Empty;
}
