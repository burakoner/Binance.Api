﻿using Binance.ApiClient.Interfaces;
using Binance.ApiClient.Models.StreamApi;

/* Unmerged change from project 'Binance.ApiClient (netstandard2.1)'
Before:
using Newtonsoft.Json;

namespace Binance.ApiClient.Models.Spot.Socket
After:
using Newtonsoft.Json;
using Binance.ApiClient.Models.zzz.Spot.Socket;
using Binance;
using Binance.ApiClient;
using Binance.ApiClient.Models.Spot;
using Binance.ApiClient.Models.Spot.Socket;

namespace Binance.ApiClient.Models.Spot.Socket
*/
using Newtonsoft.Json;

namespace Binance.ApiClient.Models.StreamApi.MarketData
{
    /// <summary>
    /// MiniTick info
    /// </summary>
    public abstract class BinanceStreamMiniTickBase : BinanceStreamEvent, IBinanceMiniTick
    {
        /// <summary>
        /// The symbol this data is for
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }

        /// <summary>
        /// The current day close price. This is the latest price for this symbol.
        /// </summary>
        [JsonProperty("c")]
        public decimal LastPrice { get; set; }

        /// <summary>
        /// Todays open price
        /// </summary>
        [JsonProperty("o")]
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// Todays high price
        /// </summary>
        [JsonProperty("h")]
        public decimal HighPrice { get; set; }

        /// <summary>
        /// Todays low price
        /// </summary>
        [JsonProperty("l")]
        public decimal LowPrice { get; set; }

        /// <summary>
        /// Total traded volume
        /// </summary>
        public abstract decimal Volume { get; set; }

        /// <summary>
        /// Total traded quote volume
        /// </summary>
        public abstract decimal QuoteVolume { get; set; }
    }

    /// <summary>
    /// Stream mini tick
    /// </summary>
    public class BinanceStreamMiniTick : BinanceStreamMiniTickBase
    {
        /// <inheritdoc/>
        [JsonProperty("v")]
        public override decimal Volume { get; set; }
        /// <inheritdoc/>
        [JsonProperty("q")]
        public override decimal QuoteVolume { get; set; }
    }

    /// <summary>
    /// Stream mini tick
    /// </summary>
    public class BinanceStreamCoinMiniTick : BinanceStreamMiniTickBase
    {
        /// <inheritdoc/>
        [JsonProperty("q")]
        public override decimal Volume { get; set; }
        /// <inheritdoc/>
        [JsonProperty("v")]
        public override decimal QuoteVolume { get; set; }
    }
}
