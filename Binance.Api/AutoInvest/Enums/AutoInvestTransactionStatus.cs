namespace Binance.Api.AutoInvest;

/// <summary>
/// Transaction status
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum AutoInvestTransactionStatus : byte
{
    /// <summary>
    /// Pending
    /// </summary>
    [Map("PENDING")]
    Pending = 1,

    /// <summary>
    /// Success
    /// </summary>
    [Map("SUCCESS")]
    Success = 2,

    /// <summary>
    /// Failed
    /// </summary>
    [Map("FAILED")]
    Failed = 3,
}
