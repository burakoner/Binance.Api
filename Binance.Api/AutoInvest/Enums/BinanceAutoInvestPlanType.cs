namespace Binance.Api.AutoInvest;

/// <summary>
/// Plan type
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceAutoInvestPlanType : byte
{
    /// <summary>
    /// All
    /// </summary>
    [Map("ALL")]
    All = 0,

    /// <summary>
    /// Single
    /// </summary>
    [Map("SINGLE")]
    Single = 1,

    /// <summary>
    /// Index
    /// </summary>
    [Map("INDEX")]
    Index = 2,

    /// <summary>
    /// Portfolio
    /// </summary>
    [Map("PORTFOLIO")]
    Portfolio = 3,
}
