using Binance.Net.Enums;

namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Result of the margin change history request
    /// </summary>
    public record BinanceFuturesMarginChangeHistoryResult
    {
        /// <summary>
        /// Request quantity of margin used
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Base asset used for margin
        /// </summary>
        [JsonProperty("asset")]
        public string? Asset { get; set; }
        /// <summary>
        /// Symbol margin is placed on
        /// </summary>
        [JsonProperty("symbol")]
        public string? Symbol { get; set; }
        /// <summary>
        /// Delta type
        /// </summary>
        [JsonProperty("deltaType")]
        public string? DeltaType { get; set; }
        /// <summary>
        /// Time of the margin change request
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Direction of the margin change request
        /// </summary>
        [JsonProperty("type")]
        public FuturesMarginChangeDirectionType Type { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonProperty("positionSide")]
        public BinancePositionSide PositionSide { get; set; }
    }

}
