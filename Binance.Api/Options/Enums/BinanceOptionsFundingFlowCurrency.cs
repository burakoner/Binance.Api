namespace Binance.Api.Options;

/// <summary>
/// Currency supported by the Options account funding-flow endpoint
/// </summary>
public enum BinanceOptionsFundingFlowCurrency : byte
{
    /// <summary>
    /// Tether USD
    /// </summary>
    [Map("USDT")]
    Usdt = 1
}
