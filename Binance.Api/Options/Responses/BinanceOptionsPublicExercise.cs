namespace Binance.Api.Options;

/// <summary>
/// Binance Options Exercise History Record
/// </summary>
public class BinanceOptionsPublicExercise
{
    /// <summary>
    /// The symbol the price is for
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Strike Price
    /// </summary>
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// Real Strike Price
    /// </summary>
    public decimal RealStrikePrice { get; set; }

    /// <summary>
    /// Expiry date
    /// </summary>
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Strike Result
    /// </summary>
    public string StrikeResult { get; set; } = "";
}