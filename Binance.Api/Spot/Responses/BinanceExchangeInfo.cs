namespace Binance.Api.Spot;

/// <summary>
/// Exchange info
/// </summary>
public record BinanceExchangeInfo
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
    public IEnumerable<BinanceRateLimit> RateLimits { get; set; } = [];

    /// <summary>
    /// Filters
    /// </summary>
    public IEnumerable<object> ExchangeFilters { get; set; } = [];

    /// <summary>
    /// All symbols supported
    /// </summary>
    public IEnumerable<BinanceSymbol> Symbols { get; set; } = [];
}
