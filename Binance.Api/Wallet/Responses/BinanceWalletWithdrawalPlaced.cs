namespace Binance.Api.Wallet;

/// <summary>
/// Result of placing a withdrawal
/// </summary>
public record BinanceWalletWithdrawalPlaced
{
    /// <summary>
    /// The id
    /// </summary>
    public string Id { get; set; } = "";
}
