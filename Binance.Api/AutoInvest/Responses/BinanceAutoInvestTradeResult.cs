namespace Binance.Api.AutoInvest;

/// <summary>
/// Trade result
/// </summary>
public record BinanceAutoInvestTradeResult
{
    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("transactionId")]
    public long? TransactionId { get; set; }

    /// <summary>
    /// Wait seconds after which the status should be checked
    /// </summary>
    [JsonProperty("waitSecond")]
    public decimal WaitSecond { get; set; }
}
