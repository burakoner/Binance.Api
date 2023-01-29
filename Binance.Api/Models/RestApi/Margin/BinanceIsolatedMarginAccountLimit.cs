namespace Binance.Api.Models.RestApi.Margin;

/// <summary>
/// Enabled account limit
/// </summary>
public class BinanceIsolatedMarginAccountLimit
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