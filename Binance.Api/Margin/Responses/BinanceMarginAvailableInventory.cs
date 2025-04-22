namespace Binance.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Margin available inventory data
    /// </summary>
    public record BinanceMarginAvailableInventory
    {
        /// <summary>
        /// Assets
        /// </summary>
        public Dictionary<string, string> Assets { get; set; } = new();
        /// <summary>
        /// Update time
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
