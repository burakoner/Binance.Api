namespace Binance.Api.Wallet;

/// <summary>
/// Wallet balance
/// </summary>
public record BinanceWalletBalance
{
    /// <summary>
    /// Is the wallet activated
    /// </summary>
    [JsonProperty("activate")]
    public bool Active { get; set; }

    /// <summary>
    /// Balance
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Name of the wallet
    /// </summary>
    public string WalletName { get; set; } = string.Empty;
}