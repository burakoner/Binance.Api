namespace Binance.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Small liability asset
    /// </summary>
    public record BinanceSmallLiabilityAsset
    {
        /// <summary>
        /// Asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Interest
        /// </summary>
        public decimal Interest { get; set; }
        /// <summary>
        /// Principal
        /// </summary>
        public decimal Principal { get; set; }
        /// <summary>
        /// Liability asset
        /// </summary>
        public string LiabilityAsset { get; set; } = string.Empty;
        /// <summary>
        /// Liability quantity
        /// </summary>
        [JsonProperty("liabilityQty")]
        public decimal LiabilityQuantity { get; set; }
    }
}
