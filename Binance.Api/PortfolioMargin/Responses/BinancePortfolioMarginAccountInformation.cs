namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Account Information
/// </summary>
public record BinancePortfolioMarginAccountInformation
{
    /// <summary>
    /// Unified MMR (Margin Maintenance Ratio)
    /// </summary>
    [JsonProperty("uniMMR")]
    public decimal UniMMR { get; set; }

    /// <summary>
    /// Account Equity
    /// </summary>
    [JsonProperty("accountEquity")]
    public decimal AccountEquity { get; set; }

    /// <summary>
    /// Actual Equity
    /// </summary>
    [JsonProperty("actualEquity")]
    public decimal ActualEquity { get; set; }

    /// <summary>
    /// Account Initial Margin
    /// </summary>
    [JsonProperty("accountInitialMargin")]
    public decimal AccountInitialMargin { get; set; }

    /// <summary>
    /// Account Maintenance Margin
    /// </summary>
    [JsonProperty("accountMaintMargin")]
    public decimal AccountMaintenanceMargin { get; set; }

    /// <summary>
    /// Account Status
    /// </summary>
    [JsonProperty("accountStatus")]
    public string AccountStatus { get; set; } = ""; // TODO: Enum

    /// <summary>
    /// Virtual Max Withdraw Amount
    /// </summary>
    [JsonProperty("virtualMaxWithdrawAmount")]
    public decimal? VirtualMaxWithdrawAmount { get; set; }

    /// <summary>
    /// Total Initial Margin
    /// </summary>
    [JsonProperty("totalAvailableBalance")]
    public decimal? TotalAvailableBalance { get; set; }

    /// <summary>
    /// Total Maintenance Margin
    /// </summary>
    [JsonProperty("totalMarginOpenLoss")]
    public decimal? TotalMarginOpenLoss { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}