namespace Binance.Api.Margin;

/// <summary>
/// Margin user data stream listen token.
/// </summary>
public record BinanceMarginListenToken
{
    /// <summary>
    /// Opaque token used by the WebSocket API subscription request.
    /// </summary>
    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration time.
    /// </summary>
    [JsonProperty("expirationTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ExpirationTime { get; set; }
}
