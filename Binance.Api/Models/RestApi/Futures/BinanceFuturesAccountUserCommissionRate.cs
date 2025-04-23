namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// User commission rate
    /// </summary>
    public record BinanceFuturesAccountUserCommissionRate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Maker commission rate
        /// </summary>
        [JsonProperty("makerCommissionRate")]
        public decimal MakerCommissionRate { get; set; }
        /// <summary>
        /// Taker commission rate
        /// </summary>
        [JsonProperty("takerCommissionRate")]
        public decimal TakerCommissionRate { get; set; }
    }
}
