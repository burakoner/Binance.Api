namespace Binance.Api.AutoInvest;

/// <summary>
/// Index status
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceAutoInvestIndexStatus : byte
{
    /// <summary>
    /// Running
    /// </summary>
    [Map("RUNNING")]
    Running = 1,

    /// <summary>
    /// Rebalancing
    /// </summary>
    [Map("REBALANCING")]
    Rebalancing = 2,

    /// <summary>
    /// Paused
    /// </summary>
    [Map("PAUSED")]
    Paused = 3,
}
