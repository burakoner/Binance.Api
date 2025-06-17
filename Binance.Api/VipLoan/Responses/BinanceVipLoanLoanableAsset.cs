namespace Binance.Api.VipLoan;

/// <summary>
/// VIP Loan Loanable Asset
/// </summary>
public record BinanceVipLoanLoanableAsset
{
    /// <summary>
    /// Loan Asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Daily Interest Rate for Flexible Loans
    /// </summary>
    [JsonProperty("_flexibleDailyInterestRate")]
    public decimal DailyInterestRateFlexible { get; set; }

    /// <summary>
    /// Yearly Interest Rate for Flexible Loans
    /// </summary>
    [JsonProperty("_flexibleYearlyInterestRate")]
    public decimal YearlyInterestRateFlexible { get; set; }

    /// <summary>
    /// Daily Interest Rate for 30-Day Loans
    /// </summary>
    [JsonProperty("_30dDailyInterestRate")]
    public decimal DailyInterest30Days { get; set; }

    /// <summary>
    /// Yearly Interest Rate for 30-Day Loans
    /// </summary>
    [JsonProperty("_30dYearlyInterestRate")]
    public decimal YearlyInterestRate30Days { get; set; }

    /// <summary>
    /// Daily Interest Rate for 60-Day Loans
    /// </summary>
    [JsonProperty("_60dDailyInterestRate")]
    public decimal DailyInterestRate60Days { get; set; }

    /// <summary>
    /// Yearly Interest Rate for 60-Day Loans
    /// </summary>
    [JsonProperty("_60dYearlyInterestRate")]
    public decimal YearlyInterestRate60Days { get; set; }

    /// <summary>
    /// Minimum Limit
    /// </summary>
    [JsonProperty("minLimit")]
    public decimal MinimumLimit { get; set; }

    /// <summary>
    /// Maximum Limit
    /// </summary>
    [JsonProperty("maxLimit")]
    public decimal MaximumLimit { get; set; }

    /// <summary>
    /// VIP Level
    /// </summary>
    [JsonProperty("vipLevel")]
    public int VipLevel { get; set; }
}
