using Binance.Api.Futures;

namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Open Interest History info
    /// </summary>
    public record BinanceFuturesCoinOpenInterestHistory
    {
        /// <summary>
        /// The symbol the information is about
        /// </summary>
        [JsonProperty("pair")]
        public string Pair { get; set; } = string.Empty;

        /// <summary>
        /// Contract type
        /// </summary>
        [JsonProperty("contractType")]
        public BinanceContractType ContractType { get; set; }

        /// <summary>
        /// Total open interest
        /// </summary>
        [JsonProperty("sumOpenInterest")]
        public decimal SumOpenInterest { get; set; }

        /// <summary>
        /// Total open interest value
        /// </summary>
        [JsonProperty("sumOpenInterestValue")]
        public decimal SumOpenInterestValue { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Timestamp { get; set; }
    }
}
