namespace Binance.Api.Options;

/// <summary>
/// Binance Options user data stream listen key
/// </summary>
public record BinanceOptionsListenKey
{
    /// <summary>
    /// Listen key used to connect to the private user data stream
    /// </summary>
    [JsonProperty("listenKey")]
    public string ListenKey { get; set; } = string.Empty;

    /// <summary>
    /// Listen key expiration timestamp in milliseconds
    /// </summary>
    [JsonProperty("expiration")]
    public long Expiration { get; set; }
}
