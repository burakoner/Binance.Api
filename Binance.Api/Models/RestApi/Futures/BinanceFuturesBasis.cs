using Binance.Net.Enums;

namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Basis info
    /// </summary>
    public record BinanceFuturesBasis
    {
        /// <summary>
        /// The pair
        /// </summary>
        [JsonProperty("pair")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonProperty("contractType")]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// Futures price
        /// </summary>
        [JsonProperty("futuresPrice")]
        public decimal FuturesPrice { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        [JsonProperty("indexPrice")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// Basis
        /// </summary>
        [JsonProperty("basis")]
        public decimal Basis { get; set; }
        /// <summary>
        /// Basis rate
        /// </summary>
        [JsonProperty("basisRate")]
        public decimal BasisRate { get; set; }
        /// <summary>
        /// Annualized basis rate
        /// </summary>
        [JsonProperty("annualizedBasisRate")]
        public decimal? AnnualizedBasisRate { get; set; }
        /// <summary>
        /// Data timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
