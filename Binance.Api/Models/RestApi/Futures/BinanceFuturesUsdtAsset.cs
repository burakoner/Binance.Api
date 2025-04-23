namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Asset info
    /// </summary>
    public record BinanceFuturesUsdtAsset
    {
        /// <summary>
        /// Name of the asset
        /// </summary>
        [JsonProperty("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Whether the asset can be used as margin in Multi-Assets mode
        /// </summary>
        [JsonProperty("marginAvailable")]
        public bool MarginAvailable { get; set; }
        /// <summary>
        /// Auto-exchange threshold in Multi-Assets margin mode
        /// </summary>
        [JsonProperty("autoAssetExchange")]
        public decimal? AutoAssetExchange { get; set; }
    }
}
