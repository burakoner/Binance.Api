namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Negative Balance Interest
/// </summary>
public record BinancePortfolioMarginNegativeBalanceInterest
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Interest
    /// </summary>
    [JsonProperty("interest")]
    public decimal Interest { get; set; }

    /// <summary>
    /// Interest Accrued Time
    /// </summary>
    [JsonProperty("interestAccuredTime")]
    public DateTime InterestAccuredTime { get; set; }

    /// <summary>
    /// Interest Rate
    /// </summary>
    [JsonProperty("interestRate")]
    public decimal InterestRate { get; set; }

    /// <summary>
    /// Principal
    /// </summary>
    [JsonProperty("principal")]
    public decimal Principal { get; set; }
}