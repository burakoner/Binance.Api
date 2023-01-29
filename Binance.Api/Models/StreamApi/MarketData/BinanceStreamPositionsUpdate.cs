using System;
using System.Collections.Generic;
using Binance.ApiClient.Interfaces;
using ApiSharp.Converters;
using Newtonsoft.Json;

/* Unmerged change from project 'Binance.ApiClient (netstandard2.1)'
Before:
using Binance.ApiClient.Models.StreamApi;
After:
using Binance.ApiClient.Models.StreamApi;
using Binance;
using Binance.ApiClient;
using Binance.ApiClient.Models;
using Binance.ApiClient.Models.zzz;
using Binance.ApiClient.Models.zzz.Spot;
using Binance.ApiClient.Models.zzz.Spot.Socket;
using Binance.ApiClient.Models.StreamApi.MarketData;
*/
using Binance.ApiClient.Models.StreamApi;

/* Unmerged change from project 'Binance.ApiClient (netstandard2.1)'
Before:
using Binance.ApiClient.Models.zzz;
After:
using Binance.ApiClient.Models.zzz;
using Binance.ApiClient.Models.zzz.Spot.Socket;
using Binance;
using Binance.ApiClient;
using Binance.ApiClient.Models;
using Binance.ApiClient.Models.Spot;
using Binance.ApiClient.Models.Spot.Socket;
*/

namespace Binance.ApiClient.Models.StreamApi.MarketData
{
    /// <summary>
    /// Positions update
    /// </summary>
    public class BinanceStreamPositionsUpdate : BinanceStreamEvent
    {
        /// <summary>
        /// Time of last account update
        /// </summary>
        [JsonProperty("u"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The listen key the update was for
        /// </summary>
        public string ListenKey { get; set; }
        /// <summary>
        /// Balances
        /// </summary>
        [JsonProperty("B")]
        public IEnumerable<BinanceStreamBalance> Balances { get; set; } = Array.Empty<BinanceStreamBalance>();
    }

    /// <summary>
    /// Information about an asset balance
    /// </summary>
    public class BinanceStreamBalance : IBinanceBalance
    {
        /// <summary>
        /// The asset this balance is for
        /// </summary>
        [JsonProperty("a")]
        public string Asset { get; set; }
        /// <summary>
        /// The quantity that isn't locked in a trade
        /// </summary>
        [JsonProperty("f")]
        public decimal Available { get; set; }
        /// <summary>
        /// The quantity that is currently locked in a trade
        /// </summary>
        [JsonProperty("l")]
        public decimal Locked { get; set; }
        /// <summary>
        /// The total balance of this asset (Free + Locked)
        /// </summary>
        public decimal Total => Available + Locked;
    }
}
