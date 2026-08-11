namespace Binance.Api.Options;

/// <summary>
/// Reason for an Options balance and position update
/// </summary>
public enum BinanceOptionsBalancePositionUpdateReason : byte
{
    /// <summary>
    /// Deposit
    /// </summary>
    [Map("DEPOSIT")]
    Deposit = 1,

    /// <summary>
    /// Withdrawal
    /// </summary>
    [Map("WITHDRAW")]
    Withdraw = 2,

    /// <summary>
    /// Order
    /// </summary>
    [Map("ORDER")]
    Order = 3
}
