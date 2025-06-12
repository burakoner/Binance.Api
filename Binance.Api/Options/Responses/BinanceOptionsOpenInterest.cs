namespace Binance.Api.Options;

/// <summary>
/// Binance Options Open Interest
/// </summary>
public class BinanceOptionsOpenInterest
{
    /// <summary>
    /// The symbol the price is for
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Sum Open Interest
    /// </summary>
    public decimal SumOpenInterest { get; set; }

    /// <summary>
    /// Sum Open Interest USD
    /// </summary>
    public decimal SumOpenInterestUsd { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }
}