using System;
using Binance.ApiClient.Interfaces;
using ApiSharp.Converters;

/* Unmerged change from project 'Binance.ApiClient (netstandard2.1)'
Before:
using Newtonsoft.Json;

namespace Binance.ApiClient.Models.Futures.Socket
After:
using Newtonsoft.Json;
using Binance.ApiClient.Models.zzz.Futures.Socket;
using Binance;
using Binance.ApiClient;
using Binance.ApiClient.Models.Futures;
using Binance.ApiClient.Models.Futures.Socket;

namespace Binance.ApiClient.Models.Futures.Socket
*/
using Newtonsoft.Json;
using Binance.ApiClient.Models.StreamApi;

namespace Binance.ApiClient.Models.zzz.Futures.Socket
{
    /// <summary>
    /// Mark price update
    /// </summary>
    public class BinanceFuturesStreamMarkPrice : BinanceStreamEvent, IBinanceFuturesMarkPrice
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }

        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonProperty("p")]
        public decimal MarkPrice { get; set; }

        /// <summary>
        /// Estimated Settle Price, only useful in the last hour before the settlement starts
        /// </summary>
        [JsonProperty("P")]
        public decimal EstimatedSettlePrice { get; set; }

        /// <summary>
        /// Next Funding Rate
        /// </summary>
        [JsonProperty("r")]
        public decimal? FundingRate { get; set; }

        /// <summary>
        /// Next Funding Time
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime NextFundingTime { get; set; }
    }

    /// <summary>
    /// Mark price update
    /// </summary>
    public class BinanceFuturesUsdtStreamMarkPrice : BinanceFuturesStreamMarkPrice
    {
        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonProperty("i")]
        public decimal IndexPrice { get; set; }
    }

    /// <summary>
    /// Mark price update
    /// </summary>
    public class BinanceFuturesCoinStreamMarkPrice : BinanceFuturesStreamMarkPrice
    {
        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonProperty("P")]
        public new decimal EstimatedSettlePrice { get; set; }
    }
}
