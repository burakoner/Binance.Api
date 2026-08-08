namespace Binance.Api.Margin;

/// <summary>
/// Result of enabling or disabling an Isolated Margin account.
/// </summary>
public record BinanceIsolatedMarginCreateAccountResult
{
    /// <summary>
    /// Success
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = string.Empty;
}
