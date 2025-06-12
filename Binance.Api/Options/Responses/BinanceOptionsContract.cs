namespace Binance.Api.Options;

/// <summary>
/// Options Contract
/// </summary>
public record BinanceOptionsContract
{
    /// <summary>
    /// Base Asset
    /// </summary>
    public string BaseAsset { get; set; } = "";

    /// <summary>
    /// Quote Asset
    /// </summary>
    public string QuoteAsset { get; set; } = "";

    /// <summary>
    /// Underlying Asset
    /// </summary>
    public string Underlying { get; set; } = "";

    /// <summary>
    /// Settle Asset
    /// </summary>
    public string settleAsset { get; set; } = "";
}
