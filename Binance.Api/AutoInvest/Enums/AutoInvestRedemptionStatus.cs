namespace Binance.Api.AutoInvest;

/// <summary>
/// Redemption status
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum AutoInvestRedemptionStatus : byte
{
    /// <summary>
    /// Failed
    /// </summary>
    [Map("FAILED")]
    Failed = 0,

    /// <summary>
    /// Success
    /// </summary>
    [Map("SUCCESS")]
    Success = 1,
}
