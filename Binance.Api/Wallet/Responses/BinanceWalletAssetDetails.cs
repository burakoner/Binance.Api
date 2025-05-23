﻿namespace Binance.Api.Wallet;

/// <summary>
/// Asset details
/// </summary>
public record BinanceWalletAssetDetails
{
    /// <summary>
    /// Minimal quantity you can withdraw
    /// </summary>
    [JsonProperty("minWithdrawAmount")]
    public decimal MinimumWithdrawalQuantity { get; set; }

    /// <summary>
    /// Whether deposits are enabled
    /// </summary>
    public bool DepositStatus { get; set; }

    /// <summary>
    /// Whether withdrawing is enabled
    /// </summary>
    public bool WithdrawStatus { get; set; }

    /// <summary>
    /// Fee for withdrawing
    /// </summary>
    public decimal WithdrawFee { get; set; }

    /// <summary>
    /// Status string for deposit
    /// </summary>
    public string DepositTip { get; set; } = "";
}
