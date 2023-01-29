using System;
using System.Collections.Generic;
using Binance.ApiClient.Converters;
using Binance.ApiClient.Enums;
using ApiSharp.Converters;

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

/* Unmerged change from project 'Binance.ApiClient (netstandard2.1)'
Before:
using Binance.ApiClient.Models.StreamApi;

namespace Binance.ApiClient.Models.zzz.Spot.Socket
After:
using Binance.ApiClient.Models.StreamApi;
using Binance;
using Binance.ApiClient;
using Binance.ApiClient.Models;
using Binance.ApiClient.Models.zzz;
using Binance.ApiClient.Models.zzz.Spot;
using Binance.ApiClient.Models.zzz.Spot.Socket;
using Binance.ApiClient.Models.StreamApi.MarketData;

namespace Binance.ApiClient.Models.StreamApi.MarketData
*/
using Binance.ApiClient.Models.StreamApi;

namespace Binance.ApiClient.Models.StreamApi.MarketData
{
    /// <summary>
    /// Order list info
    /// </summary>
    public class BinanceStreamOrderList : BinanceStreamEvent
    {
        /// <summary>
        /// The id of the order list
        /// </summary>
        [JsonProperty("g")]
        public long Id { get; set; }
        /// <summary>
        /// The contingency type
        /// </summary>
        [JsonProperty("c")]
        public string ContingencyType { get; set; }
        /// <summary>
        /// The order list status
        /// </summary>
        [JsonConverter(typeof(ListStatusTypeConverter))]
        [JsonProperty("l")]
        public ListStatusType ListStatusType { get; set; }
        /// <summary>
        /// The order status
        /// </summary>
        [JsonConverter(typeof(ListOrderStatusConverter))]
        [JsonProperty("L")]
        public ListOrderStatus ListOrderStatus { get; set; }
        /// <summary>
        /// Rejection reason
        /// </summary>
        [JsonProperty("r")]
        public string ListRejectReason { get; set; }
        /// <summary>
        /// The client id of the order list
        /// </summary>
        [JsonProperty("C")]
        public string ListClientOrderId { get; set; }
        /// <summary>
        /// The transaction time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("T")]
        public DateTime TransactionTime { get; set; }
        /// <summary>
        /// The symbol of the order list
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }
        /// <summary>
        /// The order in this list
        /// </summary>
        [JsonProperty("O")]
        public IEnumerable<BinanceStreamOrderId> Orders { get; set; } = Array.Empty<BinanceStreamOrderId>();
        /// <summary>
        /// The listen key the update was for
        /// </summary>
        public string ListenKey { get; set; }
    }

    /// <summary>
    /// Order reference
    /// </summary>
    public class BinanceStreamOrderId
    {
        /// <summary>
        /// The symbol of the order
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }
        /// <summary>
        /// The id of the order
        /// </summary>
        [JsonProperty("i")]
        public long OrderId { get; set; }
        /// <summary>
        /// The client order id
        /// </summary>
        [JsonProperty("c")]
        public string ClientOrderId { get; set; }
    }
}
