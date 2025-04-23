using Binance.Net.Enums;

namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Convert quote result
    /// </summary>
    public record BinanceFuturesQuoteResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Create time
        /// </summary>
        [JsonProperty("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonProperty("orderStatus")]
        public ConvertOrderStatus Status { get; set; }
    }


}
