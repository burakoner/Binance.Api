﻿using System;
using System.Collections.Generic;
using Binance.ApiClient.Interfaces;
using ApiSharp.Converters;
using Newtonsoft.Json;
using Binance.ApiClient.Models.RestApi.MarketData;
using Binance.ApiClient.Models.StreamApi;

namespace Binance.ApiClient.Models.zzz.Futures.Socket
{
    /// <summary>
    /// The order book for a asset
    /// </summary>
    public class BinanceFuturesStreamOrderBookDepth : BinanceStreamEvent, IBinanceFuturesEventOrderBook
    {
        /// <summary>
        /// The symbol of the order book (only filled from stream updates)
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }

        /// <summary>
        /// The time the event happened
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// The ID of the first update
        /// </summary>
        [JsonProperty("U")]
        public long? FirstUpdateId { get; set; }

        /// <summary>
        /// The ID of the last update
        /// </summary>
        [JsonProperty("u")]
        public long LastUpdateId { get; set; }


        /// <summary>
        /// The ID of the last update Id in last stream
        /// </summary>
        [JsonProperty("pu")]
        public long LastUpdateIdStream { get; set; }


        /// <summary>
        /// The list of diff bids
        /// </summary>
        [JsonProperty("b")]
        public IEnumerable<BinanceOrderBookEntry> Bids { get; set; } = Array.Empty<BinanceOrderBookEntry>();

        /// <summary>
        /// The list of diff asks
        /// </summary>
        [JsonProperty("a")]
        public IEnumerable<BinanceOrderBookEntry> Asks { get; set; } = Array.Empty<BinanceOrderBookEntry>();
    }
}