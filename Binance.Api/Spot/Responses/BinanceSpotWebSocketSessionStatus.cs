namespace Binance.Api.Spot;

/// <summary>
/// Status of a Spot WebSocket API connection session.
/// </summary>
public record BinanceSpotWebSocketSessionStatus
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

    /// <summary>
    /// Whether at least one user data stream subscription is active on this connection.
    /// </summary>
    [JsonProperty("userDataStream")]
    public bool UserDataStream { get; set; }
}
