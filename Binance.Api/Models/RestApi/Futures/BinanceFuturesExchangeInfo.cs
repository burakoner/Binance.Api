using Binance.Api.Spot;

namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// Exchange info
/// </summary>
public record BinanceFuturesExchangeInfo
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
}

/// <summary>
/// Exchange info
/// </summary>
public record BinanceFuturesUsdtExchangeInfo : BinanceFuturesExchangeInfo
{
    /// <summary>
    /// All symbols supported
    /// </summary>
    public IEnumerable<BinanceFuturesUsdtSymbol> Symbols { get; set; } = [];

    /// <summary>
    /// All assets
    /// </summary>
    public IEnumerable<BinanceFuturesUsdtAsset> Assets { get; set; } = [];
}

/// <summary>
/// Exchange info
/// </summary>
public record BinanceFuturesCoinExchangeInfo : BinanceFuturesExchangeInfo
{
    /// <summary>
    /// All symbols supported
    /// </summary>
    public IEnumerable<BinanceFuturesCoinSymbol> Symbols { get; set; } = [];
}
