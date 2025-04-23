namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Exchange info
    /// </summary>
    public record BinanceFuturesExchangeInfo
    {
        /// <summary>
        /// The timezone the server uses
        /// </summary>
        [JsonProperty("timezone")]
        public string TimeZone { get; set; } = string.Empty;
        /// <summary>
        /// The current server time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("serverTime")]
        public DateTime ServerTime { get; set; }
        /// <summary>
        /// The rate limits used
        /// </summary>
        [JsonProperty("rateLimits")]
        public IEnumerable<BinanceRateLimit> RateLimits { get; set; } = Array.Empty<BinanceRateLimit>();
        /// <summary>
        /// Filters
        /// </summary>
        [JsonProperty("exchangeFilters")]
        public IEnumerable<object> ExchangeFilters { get; set; } = Array.Empty<object>();
    }

    /// <summary>
    /// Exchange info
    /// </summary>
    public record BinanceFuturesUsdtExchangeInfo: BinanceFuturesExchangeInfo
    {
        /// <summary>
        /// All symbols supported
        /// </summary>
        [JsonProperty("symbols")]
        public IEnumerable<BinanceFuturesUsdtSymbol> Symbols { get; set; } = Array.Empty<BinanceFuturesUsdtSymbol>();

        /// <summary>
        /// All assets
        /// </summary>
        [JsonProperty("assets")]
        public IEnumerable<BinanceFuturesUsdtAsset> Assets { get; set; } = Array.Empty<BinanceFuturesUsdtAsset>();
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
        public IEnumerable<BinanceFuturesCoinSymbol> Symbols { get; set; } = Array.Empty<BinanceFuturesCoinSymbol>();
    }
}
