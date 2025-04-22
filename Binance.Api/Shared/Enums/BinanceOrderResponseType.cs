namespace Binance.Api.Shared;

/// <summary>
/// Response type
/// </summary>
public enum BinanceOrderResponseType : byte
{
    /// <summary>
    /// Ack only
    /// </summary>
    [Map("ACK")]
    Acknowledge = 1,

    /// <summary>
    /// Resulting order
    /// </summary>
    [Map("RESULT")]
    Result = 2,

    /// <summary>
    /// Full order info, only valid on SPOT orders  
    /// </summary>
    [Map("FULL")]
    Full = 3
}
