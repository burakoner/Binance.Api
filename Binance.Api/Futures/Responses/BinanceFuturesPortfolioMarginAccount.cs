namespace Binance.Api.Futures;

/// <summary>
/// Binance Futures Portfolio Margin Account
/// </summary>
public record BinanceFuturesPortfolioMarginAccount
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Maximum Withdraw Amount
    /// </summary>
    [JsonProperty("maxWithdrawAmount")]
    public decimal MaximumWithdrawAmount { get; set; }

    /// <summary>
    /// Maximum Withdraw Amount (USD)
    /// </summary>
    [JsonProperty("maxWithdrawAmountUSD")]
    public decimal MaximumWithdrawAmountUSD { get; set; }
}
