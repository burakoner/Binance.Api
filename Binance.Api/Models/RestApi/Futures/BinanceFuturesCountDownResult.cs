namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Result of setting a countdown timer
    /// </summary>
    public record BinanceFuturesCountDownResult
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Count down time in milliseconds
        /// </summary>
        [JsonProperty("countdownTime")]
        public int CountDownTime { get; set; }
    }
}
