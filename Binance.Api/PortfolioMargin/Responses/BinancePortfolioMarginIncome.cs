namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Income
/// </summary>
public record BinancePortfolioMarginIncome
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Income Type
    /// </summary>
    [JsonProperty("incomeType")]
    public string IncomeType { get; set; } = "";

    /// <summary>
    /// Income
    /// </summary>
    [JsonProperty("income")]
    public decimal Income { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Info
    /// </summary>
    [JsonProperty("info")]
    public string Info { get; set; } = "";

    /// <summary>
    /// Timestamp of the income event
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Transaction ID
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// Trade ID
    /// </summary>
    [JsonProperty("tranId")]
    public long? TradeId { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Income for CM (Coin-Margined)
/// </summary>
public record BinancePortfolioMarginIncomeCM : BinancePortfolioMarginIncome
{
}

/// <summary>
/// Binance Portfolio Margin Income for USDT-M (USDT-Margined)
/// </summary>
public record BinancePortfolioMarginIncomeUM : BinancePortfolioMarginIncome
{
}