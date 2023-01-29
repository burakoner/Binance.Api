using System;
using System.Collections.Generic;
using Binance.ApiClient.Models.StreamApi;
using Newtonsoft.Json;

namespace Binance.ApiClient.Models.zzz.Futures.Socket
{
    /// <summary>
    /// Composite index info
    /// </summary>
    public class BinanceFuturesStreamCompositeIndex : BinanceStreamEvent
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }
        /// <summary>
        /// The price
        /// </summary>
        [JsonProperty("p")]
        public decimal Price { get; set; }

        /// <summary>
        /// Composition
        /// </summary>
        [JsonProperty("c")]
        public IEnumerable<BinanceFuturesStreamCompositeIndexAsset> Composition { get; set; } = Array.Empty<BinanceFuturesStreamCompositeIndexAsset>();
    }

    /// <summary>
    /// Composite index asset info
    /// </summary>
    public class BinanceFuturesStreamCompositeIndexAsset
    {
        /// <summary>
        /// Base asset name
        /// </summary>
        [JsonProperty("b")]
        public string Asset { get; set; }
        /// <summary>
        /// Quote asset name
        /// </summary>
        [JsonProperty("q")]
        public string QuoteAsset { get; set; }
        /// <summary>
        /// Weight in quantity
        /// </summary>
        [JsonProperty("w")]
        public decimal WeightInQuantity { get; set; }
        /// <summary>
        /// Weight in percentage
        /// </summary>
        [JsonProperty("W")]
        public decimal WeightInPercentage { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        [JsonProperty("i")]
        public decimal IndexPrice { get; set; }
    }
}
