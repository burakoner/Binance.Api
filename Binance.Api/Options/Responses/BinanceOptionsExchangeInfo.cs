namespace Binance.Api.Options;

/// <summary>
/// Exchange info
/// </summary>
public record BinanceOptionsExchangeInfo
{
    /// <summary>
    /// The timezone the server uses
    /// </summary>
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
    public List<BinanceOptionsSymbol> Assets { get; set; } = [];

    /// <summary>
    /// Options contracts available on the exchange
    /// </summary>
    [JsonProperty("optionContracts")]
    public List<BinanceOptionsSymbol> Contracts { get; set; } = [];

    /// <summary>
    /// All symbols supported
    /// </summary>
    [JsonProperty("optionSymbols")]
    public List<BinanceOptionsSymbol> Symbols { get; set; } = [];

    /// <summary>
    /// The rate limits used
    /// </summary>
    public List<BinanceRateLimit> RateLimits { get; set; } = [];

}
