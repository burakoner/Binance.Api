namespace Binance.Api.SubAccount;

/// <summary>
/// Transaction
/// </summary>
public record BinanceSubAccountTransactionId
{
    /// <summary>
    /// The transaction id
    /// </summary>
    [JsonProperty("txnId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// The client transaction id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransactionId { get; set; } = string.Empty;
}
