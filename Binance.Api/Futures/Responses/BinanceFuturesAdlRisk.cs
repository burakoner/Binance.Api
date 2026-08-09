namespace Binance.Api.Futures;

/// <summary>
/// Symbol-level ADL risk rating
/// </summary>
public record BinanceFuturesAdlRisk
{
    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// ADL risk rating
    /// </summary>
    public string AdlRisk { get; set; } = string.Empty;

    /// <summary>
    /// Raw update time published by Binance
    /// </summary>
    public long UpdateTime { get; set; }
}
