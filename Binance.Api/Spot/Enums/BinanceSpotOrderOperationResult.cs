namespace Binance.Api.Spot;

/// <summary>
/// Operation result
/// </summary>
public enum BinanceSpotOrderOperationResult : byte
{
    /// <summary>
    /// Successful
    /// </summary>
    [Map("SUCCESS")]
    Success = 1,

    /// <summary>
    /// Failed
    /// </summary>
    [Map("FAILURE")]
    Failure = 2,

    /// <summary>
    /// Not attempted
    /// </summary>
    [Map("NOT_ATTEMPTED")]
    NotAttempted = 3
}
