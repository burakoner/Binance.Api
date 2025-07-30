namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Negative Balance Auto Exchange
/// </summary>
public record BinancePortfolioMarginNegativeBalanceAutoExchange
{
    /// <summary>
    /// Start Time
    /// </summary>
    [JsonProperty("startTime")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// End Time
    /// </summary>
    [JsonProperty("endTime")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Details
    /// </summary>
    [JsonProperty("details")]
    public List<BinancePortfolioMarginNegativeBalanceAutoExchangeData> Details { get; set; } = [];
}

/// <summary>
/// Binance Portfolio Margin Negative Balance Auto Exchange Data
/// </summary>
public record BinancePortfolioMarginNegativeBalanceAutoExchangeData
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Negative Balance
    /// </summary>
    [JsonProperty("negativeBalance")]
    public decimal NegativeBalance { get; set; }

    /// <summary>
    /// Negative Balance Interest
    /// </summary>
    [JsonProperty("negativeMaxThreshold")]
    public decimal NegativeMaximumThreshold { get; set; }
}