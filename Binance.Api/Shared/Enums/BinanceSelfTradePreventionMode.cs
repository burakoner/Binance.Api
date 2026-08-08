namespace Binance.Api.Shared;

/// <summary>
/// Self trade prevention mode
/// </summary>
public enum BinanceSelfTradePreventionMode : byte
{
    /// <summary>
    /// None
    /// </summary>
    [Map("NONE")]
    None = 0,

    /// <summary>
    /// Expire taker
    /// </summary>
    [Map("EXPIRE_TAKER")]
    ExpireTaker = 1,

    /// <summary>
    /// Expire maker
    /// </summary>
    [Map("EXPIRE_MAKER")]
    ExpireMaker = 2,

    /// <summary>
    /// Expire both
    /// </summary>
    [Map("EXPIRE_BOTH")]
    ExpireBoth = 3,

    /// <summary>
    /// Decrease the available quantity of both orders by the prevented quantity.
    /// </summary>
    [Map("DECREMENT")]
    Decrement = 4,

    /// <summary>
    /// Transfer prevented quantity between trade-group accounts when supported.
    /// </summary>
    [Map("TRANSFER")]
    Transfer = 5,
}
