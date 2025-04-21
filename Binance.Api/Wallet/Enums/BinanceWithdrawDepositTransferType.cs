namespace Binance.Api.Wallet;

/// <summary>
/// Transfer type
/// </summary>
public enum BinanceWithdrawDepositTransferType : byte
{
    /// <summary>
    /// External transfer
    /// </summary>
    [Map("0")]
    External = 0,

    /// <summary>
    /// Internal transfer
    /// </summary>
    [Map("1")]
    Internal = 1,
}
