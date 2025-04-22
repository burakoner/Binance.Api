namespace Binance.Api.Margin;

/// <summary>
/// Enabled account limit
/// </summary>
public record IsolatedMarginAccountLimit
{
    /// <summary>
    /// Current enabled accounts
    /// </summary>
    public int EnabledAccount { get; set; }

    /// <summary>
    /// Max accounts
    /// </summary>
    public int MaxAccount { get; set; }
}