namespace Binance.Api.Margin;

/// <summary>
/// Future hourly interest rate
/// </summary>
public record BinanceMarginInterestRate
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Next interest rate
    /// </summary>
    public decimal NextHourlyInterestRate { get; set; }
}
