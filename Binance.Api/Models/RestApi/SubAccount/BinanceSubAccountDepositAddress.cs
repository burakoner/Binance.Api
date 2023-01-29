namespace Binance.Api.Models.RestApi.SubAccount
{
    /// <summary>
    /// Deposit address info for a sub-account
    /// </summary>
    public class BinanceSubAccountDepositAddress
    {
        /// <summary>
        /// The deposit address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Asset type
        /// </summary>
        [JsonProperty("coin")]
        public string Asset { get; set; }
        /// <summary>
        /// Tag for the deposit address
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
    }
}
