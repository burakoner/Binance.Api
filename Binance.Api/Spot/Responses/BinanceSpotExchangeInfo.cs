namespace Binance.Api.Spot;

/// <summary>
/// Exchange info
/// </summary>
public record BinanceSpotExchangeInfo
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
    /// The rate limits used
    /// </summary>
    public List<BinanceRateLimit> RateLimits { get; set; } = [];

    /// <summary>
    /// Filters
    /// </summary>
    public List<object> ExchangeFilters { get; set; } = [];

    /// <summary>
    /// All symbols supported
    /// </summary>
    public List<BinanceSpotSymbol> Symbols { get; set; } = [];
}
