namespace Binance.Api.AutoInvest;

/// <summary>
/// Plan status
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceAutoInvestPlanStatus : byte
{
    /// <summary>
    /// Ongoing
    /// </summary>
    [Map("ONGOING")]
    Ongoing = 1,

    /// <summary>
    /// Paused
    /// </summary>
    [Map("PAUSED")]
    Paused = 2,

    /// <summary>
    /// Removed
    /// </summary>
    [Map("REMOVED")]
    Removed = 3
}
