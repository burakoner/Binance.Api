namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// After adjustment rate
/// </summary>
public record BinanceCrossCollateralAfterAdjust
{
    /// <summary>
    /// The rate after adjustment
    /// </summary>
    public decimal AfterCollateralRate { get; set; }
}
