﻿namespace Binance.Api.Wallet;

/// <summary>
/// The result of transferring
/// </summary>
public record BinanceWalletTransaction
{
    /// <summary>
    /// The Transaction id as assigned by Binance
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }
}
