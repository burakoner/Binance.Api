using Binance.Net.Enums;

namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Open interest
    /// </summary>
    public record BinanceFuturesOpenInterest
    {
        /// <summary>
        /// The symbol the information is about
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Open Interest info
        /// </summary>
        [JsonProperty("openInterest")]
        public decimal OpenInterest { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Timestamp { get; set; }
    }

    /// <summary>
    /// Open interest
    /// </summary>
    public record BinanceFuturesCoinOpenInterest: BinanceFuturesOpenInterest
    {
        /// <summary>
        /// The pair
        /// </summary>
        [JsonProperty("pair")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// The contract type
        /// </summary>
        [JsonProperty("contractType")]
        public ContractType ContractType { get; set; }
    }

}