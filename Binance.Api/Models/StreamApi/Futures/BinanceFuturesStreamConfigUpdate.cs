using System;
using Binance.ApiClient.Converters;
using Binance.ApiClient.Enums;
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
    /// Information about leverage of symbol changed
    /// </summary>
    public class BinanceFuturesStreamConfigUpdate : BinanceStreamEvent
    {
        /// <summary>
        /// Leverage Update data
        /// </summary>
        [JsonProperty("ac")]
        public BinanceFuturesStreamLeverageUpdateData LeverageUpdateData { get; set; } = new BinanceFuturesStreamLeverageUpdateData();

        /// <summary>
        /// Position mode Update data
        /// </summary>
        [JsonProperty("ai")]
        public BinanceFuturesStreamConfigUpdateData ConfigUpdateData { get; set; } = new BinanceFuturesStreamConfigUpdateData();

        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }
        /// <summary>
        /// The listen key the update was for
        /// </summary>
        public string ListenKey { get; set; }
    }

    /// <summary>
    /// Config update data
    /// </summary>
    public class BinanceFuturesStreamLeverageUpdateData
    {
        /// <summary>
        /// The symbol this balance is for
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }

        /// <summary>
        /// The symbol this leverage is for
        /// </summary>
        [JsonProperty("l")]
        public int Leverage { get; set; }
    }

    /// <summary>
    /// Position mode update data
    /// </summary>
    public class BinanceFuturesStreamConfigUpdateData
    {
        /// <summary>
        /// Multi-Assets Mode
        /// </summary>
        [JsonProperty("j"), JsonConverter(typeof(PositionModeConverter))]
        public PositionMode PositionMode { get; set; }
    }
}
