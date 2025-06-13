namespace Binance.Api.Link;

/// <summary>
/// Income Type
/// </summary>
public enum BinanceLinkIncomeType : byte
{
    /// <summary> 
    /// Transfer 
    /// </summary>
    [Map("TRANSFER")]
    Transfer = 1,

    /// <summary> 
    /// Welcome Bonus
    /// </summary>
    [Map("WELCOME_BONUS")]
    WelcomeBonus = 2,

    /// <summary>
    /// Realized Pnl
    /// </summary>
    [Map("REALIZED_PNL")]
    RealizedPnl = 3,

    /// <summary>
    /// FundingFee
    /// </summary>
    [Map("FUNDING_FEE")]
    FundingFee = 4,

    /// <summary>
    /// Commission
    /// </summary>
    [Map("COMMISSION")]
    Commission = 5,

    /// <summary>
    /// Insurance Clear
    /// </summary>
    [Map("INSURANCE_CLEAR")]
    InsuranceClear = 6
}