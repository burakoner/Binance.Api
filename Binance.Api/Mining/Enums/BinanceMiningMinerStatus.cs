namespace Binance.Api.Mining;

/// <summary>
/// Miner status
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceMiningMinerStatus : byte
{
    /// <summary>
    /// All miners
    /// </summary>
    [Map("0")]
    All = 0,
    
    /// <summary>
    /// Valid
    /// </summary>
    [Map("1")]
    Valid = 1,
    
    /// <summary>
    /// Invalid
    /// </summary>
    [Map("2")]
    Invalid = 2,

    /// <summary>
    /// Failure
    /// </summary>
    [Map("3")]
    Failure = 3
}
