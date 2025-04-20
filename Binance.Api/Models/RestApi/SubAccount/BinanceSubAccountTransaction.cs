namespace Binance.Api.Models.RestApi.SubAccount;

/// <summary>
/// Transaction
/// </summary>
public record BinanceSubAccountTransaction
{
    /// <summary>
    /// The transaction id
    /// </summary>
    [JsonProperty("txnId")]
    public string TransactionId { get; set; } = "";
}
