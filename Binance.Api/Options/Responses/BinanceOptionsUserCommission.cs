namespace Binance.Api.Options;

/// <summary>
/// Options account commission rates
/// </summary>
public record BinanceOptionsUserCommission
{
    /// <summary>
    /// Commission rates by underlying
    /// </summary>
    public List<BinanceOptionsUserCommissionRate> Commissions { get; set; } = [];
}

/// <summary>
/// Options commission rates for an underlying
/// </summary>
public record BinanceOptionsUserCommissionRate
{
    /// <summary>
    /// Underlying asset
    /// </summary>
    public string Underlying { get; set; } = "";

    /// <summary>
    /// Maker fee rate
    /// </summary>
    public decimal MakerFee { get; set; }

    /// <summary>
    /// Taker fee rate
    /// </summary>
    public decimal TakerFee { get; set; }
}
