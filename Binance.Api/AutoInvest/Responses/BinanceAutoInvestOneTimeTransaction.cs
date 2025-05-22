namespace Binance.Api.AutoInvest;

/// <summary>
/// Transaction status
/// </summary>
public record BinanceAutoInvestOneTimeTransaction
{
    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("transactionId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceAutoInvestOneTimeStatus Status { get; set; }
}
