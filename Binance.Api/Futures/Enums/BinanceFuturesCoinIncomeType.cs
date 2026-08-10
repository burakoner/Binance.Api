namespace Binance.Api.Futures;

/// <summary>
/// Type of COIN-M futures income
/// </summary>
public enum BinanceFuturesCoinIncomeType : byte
{
    /// <summary>
    /// Transfer into or out of the account
    /// </summary>
    [Map("TRANSFER")]
    Transfer,

    /// <summary>
    /// Futures welcome bonus
    /// </summary>
    [Map("WELCOME_BONUS")]
    WelcomeBonus,

    /// <summary>
    /// Futures funding fee
    /// </summary>
    [Map("FUNDING_FEE")]
    FundingFee,

    /// <summary>
    /// Futures realized profit or loss
    /// </summary>
    [Map("REALIZED_PNL")]
    RealizedPnl,

    /// <summary>
    /// Futures trading commission
    /// </summary>
    [Map("COMMISSION")]
    Commission,

    /// <summary>
    /// Insurance clear
    /// </summary>
    [Map("INSURANCE_CLEAR")]
    InsuranceClear,

    /// <summary>
    /// Delivered settlement
    /// </summary>
    [Map("DELIVERED_SETTELMENT")]
    DeliveredSettlement
}
