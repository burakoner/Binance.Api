namespace Binance.Api.SubAccount;

/// <summary>
/// Transfer type
/// </summary>
public enum BinanceSubAccountTransferType : byte
{
    /// <summary>
    /// From main spot account to sub account
    /// </summary>
    [Map("1")]
    TransferIn = 1,

    /// <summary>
    /// From sub account to main spot account
    /// </summary>
    [Map("2")]
    TransferOut = 2
}
