namespace Binance.Api.Models.RestApi.Margin;

/// <summary>
/// Result of creating isolated margin account
/// </summary>
public record BinanceCreateIsolatedMarginAccountResult
{
    /// <summary>
    /// Success
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";
}
