namespace Binance.Api.Spot.Enums;

/// <summary>
/// Response type
/// </summary>
public enum BinanceSpotOrderResponseType:byte
{
    /// <summary>
    /// Ack only
    /// </summary>
    [Map("ACK")]
    Acknowledge=1,

    /// <summary>
    /// Resulting order
    /// </summary>
    [Map("RESULT")]
    Result,

    /// <summary>
    /// Full order info, only valid on SPOT orders  
    /// </summary>
    [Map("FULL")]
    Full
}
