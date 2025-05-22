namespace Binance.Api.AutoInvest;

/// <summary>
/// Execution type
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceAutoInvestExecutionType : byte
{
    /// <summary>
    /// One time
    /// </summary>
    [Map("ONE_TIME")]
    OneTime = 1,

    /// <summary>
    /// Recurring
    /// </summary>
    [Map("RECURRING")]
    Recurring = 2
}
