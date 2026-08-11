namespace Binance.Api.Options;

/// <summary>
/// Exchange info
/// </summary>
public record BinanceOptionsExchangeInfo
{
    /// <summary>
    /// The timezone the server uses
    /// </summary>
    [JsonProperty("timezone")]
    public string TimeZone { get; set; } = "";

    /// <summary>
    /// The current server time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ServerTime { get; set; }

    /// <summary>
    /// Options assets available on the exchange
    /// </summary>
    [JsonProperty("optionAssets")]
    public List<BinanceOptionsAsset> Assets { get; set; } = [];

    /// <summary>
    /// Options contracts available on the exchange
    /// </summary>
    [JsonProperty("optionContracts")]
    public List<BinanceOptionsContract> Contracts { get; set; } = [];

    /// <summary>
    /// All symbols supported
    /// </summary>
    [JsonProperty("optionSymbols")]
    public List<BinanceOptionsSymbol> Symbols { get; set; } = [];

    /// <summary>
    /// The rate limits used
    /// </summary>
    public List<BinanceOptionsRateLimit> RateLimits { get; set; } = [];
}

/// <summary>
/// Options request rate-limit information
/// </summary>
public record BinanceOptionsRateLimit
{
    /// <summary>
    /// Rate-limit type
    /// </summary>
    public string RateLimitType { get; set; } = "";

    /// <summary>
    /// Rate-limit interval
    /// </summary>
    public string Interval { get; set; } = "";

    /// <summary>
    /// Number of intervals
    /// </summary>
    public long IntervalNum { get; set; }

    /// <summary>
    /// Request limit
    /// </summary>
    public long Limit { get; set; }
}
