using Binance.Api.Futures.Responses;

namespace Binance.Api.Futures;

/// <summary>
/// Exchange info
/// </summary>
public abstract record BinanceFuturesExchangeInfo
{
    /// <summary>
    /// The timezone the server uses
    /// </summary>
    public string TimeZone { get; set; } = string.Empty;

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
    /// All assets
    /// </summary>
    public IEnumerable<BinanceFuturesUsdtAsset> Assets { get; set; } = [];

    /// <summary>
    /// All symbols supported
    /// </summary>
    public IEnumerable<BinanceFuturesUsdtSymbol> Symbols { get; set; } = [];
}

/// <summary>
/// Exchange info
/// </summary>
public record BinanceFuturesCoinExchangeInfo : BinanceFuturesExchangeInfo
{
    /// <summary>
    /// All symbols supported
    /// </summary>
    [JsonProperty("symbols")]
    public IEnumerable<BinanceFuturesCoinSymbol> Symbols { get; set; } = [];
}

/// <summary>
/// Asset info
/// </summary>
public record BinanceFuturesUsdtAsset
{
    /// <summary>
    /// Name of the asset
    /// </summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Whether the asset can be used as margin in Multi-Assets mode
    /// </summary>
    public bool MarginAvailable { get; set; }

    /// <summary>
    /// Auto-exchange threshold in Multi-Assets margin mode
    /// </summary>
    public decimal? AutoAssetExchange { get; set; }
}
