namespace Binance.Api.Wallet;

/// <summary>
/// User balance
/// </summary>
public record BinanceWalletUserBalance : BinanceWalletSpotAccountBalance
{
    /// <summary>
    /// Frozen
    /// </summary>
    public decimal Freeze { get; set; }

    /// <summary>
    /// Currently withdrawing
    /// </summary>
    public decimal Withdrawing { get; set; }

    /// <summary>
    /// Ipoable amount
    /// </summary>
    public decimal Ipoable { get; set; }

    /// <summary>
    /// Value in btc
    /// </summary>
    public decimal BtcValuation { get; set; }
}
