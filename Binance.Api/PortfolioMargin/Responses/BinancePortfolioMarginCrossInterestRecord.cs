namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Cross Interest Record
/// </summary>
public record BinancePortfolioMarginCrossInterestRecord
{
    /// <summary>
    /// Transaction Time
    /// </summary>
    [JsonProperty("txId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// Interest Accrued Time
    /// </summary>
    [JsonProperty("interestAccuredTime")]
    public DateTime InterestAccuredTime { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Raw Asset
    /// </summary>
    [JsonProperty("rawAsset")]
    public string RawAsset { get; set; } = "";

    /// <summary>
    /// Principal
    /// </summary>
    [JsonProperty("principal")]
    public decimal Principal { get; set; }

    /// <summary>
    /// Interest
    /// </summary>
    [JsonProperty("interest")]
    public decimal Interest { get; set; }

    /// <summary>
    /// Interest Rate
    /// </summary>
    [JsonProperty("interestRate")]
    public decimal InterestRate { get; set; }

    /// <summary>
    /// Type
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = ""; // TODO: Enum
}