namespace Binance.Api.Shared;

/// <summary>
/// Trade rules behaviour
/// </summary>
public enum BinanceTradeRulesBehavior : byte
{
    /// <summary>
    /// None
    /// </summary>
    None = 0,

    /// <summary>
    /// Throw an error if not complying
    /// </summary>
    ThrowError = 1,

    /// <summary>
    /// Auto adjust order when not complying
    /// </summary>
    AutoComply = 2
}
