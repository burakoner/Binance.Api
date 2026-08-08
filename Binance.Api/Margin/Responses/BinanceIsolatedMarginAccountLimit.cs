namespace Binance.Api.Margin;

/// <summary>
/// Enabled account limit
/// </summary>
public record BinanceIsolatedMarginAccountLimit
{
    /// <summary>
    /// Current enabled accounts
    /// </summary>
    public long EnabledAccount { get; set; }

    /// <summary>
    /// Max accounts
    /// </summary>
    public long MaxAccount { get; set; }
}
