namespace Binance.Api.Fiat;

/// <summary>
/// Binance Fiat Payment Type
/// </summary>
public enum BinanceFiatPaymentType : byte
{
    /// <summary>
    /// Buy
    /// </summary>
    [Map("buy")]
    Buy = 1,

    /// <summary>
    /// Sell
    /// </summary>>
    [Map("sell")]
    Sell = 2,
}
