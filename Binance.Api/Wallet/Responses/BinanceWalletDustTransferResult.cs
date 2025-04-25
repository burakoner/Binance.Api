﻿namespace Binance.Api.Wallet;

/// <summary>
/// Result of dust transfer
/// </summary>
public record BinanceWalletDustTransferResult
{
    /// <summary>
    /// Total service charge
    /// </summary>
    public decimal TotalServiceCharge { get; set; }

    /// <summary>
    /// Total transferred
    /// </summary>
    [JsonProperty("totalTransfered")]
    public decimal TotalTransferred { get; set; }

    /// <summary>
    /// Transfer entries
    /// </summary>
    public IEnumerable<BinanceDustTransferResultEntry> TransferResult { get; set; } = [];
}

/// <summary>
/// Dust transfer entry
/// </summary>
public record BinanceDustTransferResultEntry
{
    /// <summary>
    /// Quantity of dust
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    public string FromAsset { get; set; } = "";

    /// <summary>
    /// Timestamp of conversion
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter)), JsonProperty("operateTime")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Service charge
    /// </summary>
    [JsonProperty("serviceChargeAmount")]
    public decimal ServiceChargeQuantity { get; set; }

    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// BNB result quantity
    /// </summary>
    [JsonProperty("transferedAmount")]
    public decimal TransferredQuantity { get; set; }
}
