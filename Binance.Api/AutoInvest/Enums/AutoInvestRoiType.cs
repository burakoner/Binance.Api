namespace Binance.Api.AutoInvest;

/// <summary>
/// Auto invest ROI type
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum AutoInvestRoiType : int
{
    /// <summary>
    /// Seven days
    /// </summary>
    [Map("SEVEN_DAY")]
    SevenDay = 7,

    /// <summary>
    /// Three months
    /// </summary>
    [Map("THREE_MONTH")]
    ThreeMonth = 90,

    /// <summary>
    /// Six months
    /// </summary>
    [Map("SIX_MONTH")]
    SixMonth = 180,

    /// <summary>
    /// One year
    /// </summary>
    [Map("ONE_YEAR")]
    OneYear = 365,

    /// <summary>
    /// Three year
    /// </summary>
    [Map("THREE_YEAR")]
    ThreeYear = 1095,

    /// <summary>
    /// Five year
    /// </summary>
    [Map("FIVE_YEAR")]
    FiveYear = 1825,
}
