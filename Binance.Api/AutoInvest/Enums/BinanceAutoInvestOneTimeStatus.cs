namespace Binance.Api.AutoInvest;

/// <summary>
/// Transaction status
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceAutoInvestOneTimeStatus : byte
{
    /// <summary>
    /// Success
    /// </summary>
    [Map("SUCCESS")]
    Success = 1,

    /// <summary>
    /// Converting
    /// </summary>
    [Map("CONVERTING")]
    Converting = 2
}
