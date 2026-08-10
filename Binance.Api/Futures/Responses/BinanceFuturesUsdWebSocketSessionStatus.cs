namespace Binance.Api.Futures;

/// <summary>
/// Status of a USDⓈ-M Futures WebSocket API connection session.
/// </summary>
public record BinanceFuturesUsdWebSocketSessionStatus
{
    /// <summary>
    /// API key authenticated on the connection, or null when the connection is not authenticated.
    /// </summary>
    [JsonProperty("apiKey")]
    public string? ApiKey { get; set; }

    /// <summary>
    /// Time at which the API key was authenticated, or null when the connection is not authenticated.
    /// </summary>
    [JsonProperty("authorizedSince"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? AuthorizedSince { get; set; }

    /// <summary>
    /// Time at which the WebSocket connection was established.
    /// </summary>
    [JsonProperty("connectedSince"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ConnectedSince { get; set; }

    /// <summary>
    /// Whether responses on this connection include rate-limit data.
    /// </summary>
    [JsonProperty("returnRateLimits")]
    public bool ReturnRateLimits { get; set; }

    /// <summary>
    /// Binance server time reported with the session status.
    /// </summary>
    [JsonProperty("serverTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ServerTime { get; set; }
}

/// <summary>
/// Binance notification that the API key authenticated on a USDⓈ-M WebSocket session was revoked.
/// </summary>
public record BinanceFuturesUsdWebSocketSessionRevocation
{
    /// <summary>
    /// WebSocket API response status.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Binance error code.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Binance error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
