namespace Binance.Api.Enums;

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
    ThrowError,

    /// <summary>
    /// Auto adjust order when not complying
    /// </summary>
    AutoComply
}
