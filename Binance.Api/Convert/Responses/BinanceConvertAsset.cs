namespace Binance.Api.Convert;

/// <summary>
/// Binance Convert Exchange Info Asset
/// </summary>
public record BinanceConvertAsset
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// Fraction
    /// </summary>
    public int Fraction { get; set; }
}
