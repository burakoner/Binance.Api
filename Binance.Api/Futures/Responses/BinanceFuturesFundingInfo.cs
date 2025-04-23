namespace Binance.Api.Futures;

/// <summary>
/// Funding rate information for Futures trading
/// </summary>
public record BinanceFuturesFundingInfo
{
    /// <summary>
    /// The symbol the information is about
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Adjusted funding rate cap
    /// </summary>
    public decimal AdjustedFundingRateCap { get; set; }

    /// <summary>
    /// Adjusted funding rate floor
    /// </summary>
    public decimal AdjustedFundingRateFloor { get; set; }

    /// <summary>
    /// Funding interval in hours
    /// </summary>
    public int FundingIntervalHours { get; set; }
}
