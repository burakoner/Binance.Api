﻿namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// Result of a convert transfer operation
/// </summary>
public record BinanceConvertTransferRecord
{
    /// <summary>
    /// Transfer id
    /// </summary>
    [JsonProperty("tranId")]
    public long TransferId { get; set; }
    /// <summary>
    /// Status of the transfer (definitions currently unknown)
    /// </summary>
    public string Status { get; set; } = "";
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
    /// <summary>
    /// Type
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// Deducted asset
    /// </summary>
    public string DeductedAsset { get; set; } = "";
    /// <summary>
    /// Deducted quantity
    /// </summary>
    [JsonProperty("deductedAmount")]
    public decimal DeductedQuantity { get; set; }
    /// <summary>
    /// Target asset
    /// </summary>
    public string TargetAsset { get; set; } = "";
    /// <summary>
    /// Target quantity
    /// </summary>
    [JsonProperty("targetAmount")]
    public decimal TargetQuantity { get; set; }
    /// <summary>
    /// Account type
    /// </summary>
    public string AccountType { get; set; } = "";
}
