namespace Binance.Api.Models.RestApi.SubAccount
{
    /// <summary>
    /// Transaction
    /// </summary>
    public class BinanceSubAccountTransaction
    {
        /// <summary>
        /// The transaction id
        /// </summary>
        [JsonProperty("txnId")]
        public string TransactionId { get; set; }
    }
}
