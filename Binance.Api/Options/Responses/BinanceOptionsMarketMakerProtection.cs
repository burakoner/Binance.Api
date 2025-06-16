namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Protection Configuration
/// </summary>
public record BinanceOptionsMarketMakerProtection
{
    /// <summary>
    /// Underlying Id
    /// </summary>
    public long UnderlyingId { get; set; }

    /// <summary>
    /// Underlying
    /// </summary>
    public string Underlying { get; set; } = "";

    /// <summary>
    /// Window time in milliseconds
    /// </summary>
    [JsonProperty("windowTimeInMilliseconds")]
    public int WindowTimeInMilliseconds { get; set; }

    /// <summary>
    /// Frozen time in milliseconds
    /// </summary>
    [JsonProperty("frozenTimeInMilliseconds")]
    public int FrozenTimeInMilliseconds { get; set; }

    /// <summary>
    /// Quantity limit
    /// </summary>
    [JsonProperty("qtyLimit")]
    public decimal QuantityLimit { get; set; }

    /// <summary>
    /// Delta limit
    /// </summary>
    [JsonProperty("deltaLimit")]
    public decimal DeltaLimit { get; set; }

    /// <summary>
    /// Last trigger time in milliseconds
    /// </summary>
    [JsonProperty("lastTriggerTime")]
    public long LastTriggerTime { get; set; }
}