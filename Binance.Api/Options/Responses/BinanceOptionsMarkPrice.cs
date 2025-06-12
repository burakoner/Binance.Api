namespace Binance.Api.Options;

/// <summary>
/// Options Mark Price
/// </summary>
public class BinanceOptionsMarkPrice
{
    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Mark Price
    /// </summary>
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Implied volatility Buy
    /// </summary>
    public decimal BidIV { get; set; }

    /// <summary>
    /// Implied volatility Sell
    /// </summary>
    public decimal AskIV { get; set; }

    /// <summary>
    /// Implied volatility Mark
    /// </summary>
    public decimal MarkIV { get; set; }

    /// <summary>
    /// Delta
    /// </summary>
    public decimal Delta { get; set; }

    /// <summary>
    /// Theta
    /// </summary>
    public decimal Theta { get; set; }

    /// <summary>
    /// Gamma
    /// </summary>
    public decimal Gamma { get; set; }

    /// <summary>
    /// Vega
    /// </summary>
    public decimal Vega { get; set; }

    /// <summary>
    /// High Price Limit
    /// </summary>
    public decimal HighPriceLimit { get; set; }

    /// <summary>
    /// Low Price Limit
    /// </summary>
    public decimal LowPriceLimit { get; set; }

    /// <summary>
    /// Risk Free Rate
    /// </summary>
    public decimal RiskFreeInterest { get; set; }
}