namespace Binance.Api.VipLoan;

/// <summary>
/// VIP Loan Interest Rate
/// </summary>
public record BinanceVipLoanInterestRate
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Flexible Daily Interest Rate
    /// </summary>
    public decimal FlexibleDailyInterestRate { get; set; }

    /// <summary>
    /// Flexible Yearly Interest Rate
    /// </summary>
    public decimal FlexibleYearlyInterestRate { get; set; }

    /// <summary>
    /// Time
    /// </summary>
    public DateTime Time { get; set; }
}
