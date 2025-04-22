namespace Binance.Api.Margin;

/// <summary>
/// Direction of a transfer
/// </summary>
public enum BinanceTransferDirection : byte
{
    /// <summary>
    /// Roll-in
    /// </summary>
    [Map("ROLL_IN")]
    RollIn = 1,

    /// <summary>
    /// Roll-out
    /// </summary>
    [Map("ROLL_OUT")]
    RollOut = 2
}
