namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Commission Rate
/// </summary>
public record BinancePortfolioMarginCommissionRate
{
    /// <summary>
    /// Symbol for which the commission rate applies
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Maker commission rate
    /// </summary>
    [JsonProperty("makerCommissionRate")]
    public decimal MakerCommissionRate { get; set; }

    /// <summary>
    /// Taker commission rate
    /// </summary>
    [JsonProperty("takerCommissionRate")]
    public decimal TakerCommissionRate { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Commission Rate for CM (Coin-Margined)
/// </summary>
public record BinancePortfolioMarginCommissionRateCM : BinancePortfolioMarginCommissionRate
{
}

/// <summary>
/// Binance Portfolio Margin Commission Rate for UM (USD-Margined)
/// </summary>
public record BinancePortfolioMarginCommissionRateUM : BinancePortfolioMarginCommissionRate
{
}