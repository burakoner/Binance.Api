using Binance.Net.Enums;

namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Futures income history result
    /// </summary>
    public record BinanceFuturesIncomeHistory
    {
        /// <summary>
        /// Symbol for the resulting income history, may be null if not associated with a trading pair
        /// </summary>
        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        /// <summary>
        /// Type of income as string
        /// </summary>
        [JsonProperty("incomeType")]
        public IncomeType? IncomeType { get; set; }

        /// <summary>
        /// Quantity of income
        /// </summary>
        [JsonProperty("income")]
        public decimal Income { get; set; }
        /// <summary>
        /// Base asset for the income
        /// </summary>
        [JsonProperty("asset")]
        public string? Asset { get; set; }
        /// <summary>
        /// Additional info
        /// </summary>
        [JsonProperty("info")]
        public string? Info { get; set; }
        /// <summary>
        /// Time of the income
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("time")]
        public DateTime Timestamp { get; set; }
        /*
        /// <summary>
        /// Transaction id if relevant
        /// </summary>
        [JsonProperty("tranId")]
        [JsonConverter(typeof(NumberStringConverter))]
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// Trade id if existing
        /// </summary>
        [JsonProperty("tradeId")]
        [JsonConverter(typeof(NumberStringConverter))]
        public string? TradeId { get; set; }
        */
    }

}
