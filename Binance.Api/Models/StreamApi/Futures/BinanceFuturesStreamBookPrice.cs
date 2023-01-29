namespace Binance.Api.Models.StreamApi.Futures
{
    /// <summary>
    /// Futures book price
    /// </summary>
    public class BinanceFuturesStreamBookPrice : BinanceStreamBookPrice
    {

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? TransactionTime { get; set; }
        /// <summary>
        /// The time the event happened
        /// </summary>
        [JsonProperty("E"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime EventTime { get; set; }

        [JsonProperty("e")] private string Event { get; set; }
    }
}
