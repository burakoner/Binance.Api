using Binance.ApiClient.Models.StreamApi;
using Newtonsoft.Json;

namespace Binance.ApiClient.Models.zzz.Futures.Socket
{
    /// <summary>
    /// Index price update
    /// </summary>
    public class BinanceFuturesStreamIndexPrice : BinanceStreamEvent
    {
        /// <summary>
        /// The pair
        /// </summary>
        [JsonProperty("i")]
        public string Pair { get; set; }
        /// <summary>
        /// The index price
        /// </summary>
        [JsonProperty("p")]
        public decimal IndexPrice { get; set; }
    }
}
