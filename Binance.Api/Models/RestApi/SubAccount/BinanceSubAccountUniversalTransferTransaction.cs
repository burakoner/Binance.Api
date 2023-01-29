using System;
using System.Collections.Generic;
using Binance.ApiClient.Converters;
using Binance.ApiClient.Enums;
using ApiSharp.Converters;

/* Unmerged change from project 'Binance.ApiClient (netstandard2.1)'
Before:
using Newtonsoft.Json;

namespace Binance.ApiClient.Models.Spot.SubAccountData
After:
using Newtonsoft.Json;
using Binance.ApiClient.Models.zzz.Spot.SubAccountData;
using Binance;
using Binance.ApiClient;
using Binance.ApiClient.Models;
using Binance.ApiClient.Models.Spot;
using Binance.ApiClient.Models.Spot.SubAccountData;

namespace Binance.ApiClient.Models.Spot.SubAccountData
*/

/* Unmerged change from project 'Binance.ApiClient (netstandard2.1)'
Before:
using Newtonsoft.Json;

namespace Binance.ApiClient.Models.zzz.Spot.SubAccountData
After:
using Newtonsoft.Json;
using Binance;
using Binance.ApiClient;
using Binance.ApiClient.Models;
using Binance.ApiClient.Models.zzz;
using Binance.ApiClient.Models.zzz.Spot;
using Binance.ApiClient.Models.zzz.Spot.SubAccountData;
using Binance.ApiClient.Models.RestApi.SubAccount;

namespace Binance.ApiClient.Models.RestApi.SubAccount
*/
using Newtonsoft.Json;

namespace Binance.ApiClient.Models.RestApi.SubAccount
{
    internal class BinanceSubAccountUniversalTransfersList
    {
        /// <summary>
        /// Transactions
        /// </summary>
        [JsonProperty("result")]
        public IEnumerable<BinanceSubAccountUniversalTransferTransaction> Transactions { get; set; } =
            new List<BinanceSubAccountUniversalTransferTransaction>();

    }

    /// <summary>
    /// Binance sub account universal transaction
    /// </summary>
    public class BinanceSubAccountUniversalTransferTransaction
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonProperty("tranId")]
        public long TransactionId { get; set; }

        /// <summary>
        /// From email
        /// </summary>
        public string FromEmail { get; set; } = "";

        /// <summary>
        /// To email
        /// </summary>
        public string ToEmail { get; set; } = "";

        /// <summary>
        /// From account type
        /// </summary>
        [JsonConverter(typeof(BrokerageAccountTypeConverter))]
        public BrokerageAccountType FromAccountType { get; set; }

        /// <summary>
        /// To account type
        /// </summary>
        [JsonConverter(typeof(BrokerageAccountTypeConverter))]
        public BrokerageAccountType ToAccountType { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; } = "";

        /// <summary>
        /// Asset
        /// </summary>
        public string Asset { get; set; } = "";

        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// The time the universal transaction was created
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("createTimeStamp")]
        public DateTime CreateTime { get; set; }
    }
}
