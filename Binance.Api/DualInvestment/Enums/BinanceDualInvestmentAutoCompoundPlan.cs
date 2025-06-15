namespace Binance.Api.DualInvestment;

/// <summary>
/// Binance Dual Investment Auto Compound Plan
/// </summary>
public enum BinanceDualInvestmentAutoCompoundPlan : byte
{
    /// <summary>
    /// Switch Off the Plan
    /// </summary>
    [Map("NONE")]
    None = 0,

    /// <summary>
    /// Standard Plan
    /// </summary>>
    [Map("STANDARD")]
    Standard = 1,

    /// <summary>
    /// Advanced Plan
    /// </summary>>
    [Map("ADVANCED")]
    Advanced = 2,
}
