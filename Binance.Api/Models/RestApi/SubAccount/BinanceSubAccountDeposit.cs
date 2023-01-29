using System;
using Binance.ApiClient.Converters;
using Binance.ApiClient.Enums;
using ApiSharp.Converters;
using Newtonsoft.Json;

namespace Binance.ApiClient.Models.RestApi.SubAccount
{
    /// <summary>
    /// Information about a deposit
    /// </summary>
    public class BinanceSubAccountDeposit
    {
        /// <summary>
        /// Time the deposit was added to Binance
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// The quantity deposited
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The asset deposited
        /// </summary>
        [JsonProperty("coin")]
        public string Asset { get; set; }
        /// <summary>
        /// Network
        /// </summary>
        public string Network { get; set; }
        /// <summary>
        /// The address of the deposit
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// The address tag
        /// </summary>
        public string AddressTag { get; set; }
        /// <summary>
        /// The transaction id
        /// </summary>
        [JsonProperty("txId")]
        public string TransactionId { get; set; }
        /// <summary>
        /// Confirmation status
        /// </summary>
        public string ConfirmTimes { get; set; }
        /// <summary>
        /// Transfer type
        /// </summary>
        public int TransferType { get; set; }
        /// <summary>
        /// The status of the deposit
        /// </summary>
        [JsonConverter(typeof(DepositStatusConverter))]
        public DepositStatus Status { get; set; }
    }
}
