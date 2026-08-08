namespace Binance.Api.Margin;

/// <summary>
/// Permission mode for an Ed25519 Margin Special Key.
/// </summary>
public enum BinanceMarginSpecialKeyPermissionMode : byte
{
    /// <summary>
    /// Trading permissions.
    /// </summary>
    [Map("TRADE")]
    Trade = 1,

    /// <summary>
    /// USER_DATA and FIX_API_READ_ONLY permissions.
    /// </summary>
    [Map("READ")]
    Read = 2
}
