﻿using Binance.Api.Margin;

namespace Binance.Api.Wallet;

/// <summary>
/// Margin account snapshot
/// </summary>
public record BinanceMarginAccountSnapshot
{
    /// <summary>
    /// Timestamp of the data
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter)), JsonProperty("updateTime")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Account type the data is for
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinancePermissionType Type { get; set; }

    /// <summary>
    /// Snapshot data
    /// </summary>
    [JsonProperty("data")]
    public BinanceMarginAccountSnapshotData Data { get; set; } = default!;
}

/// <summary>
/// Margin snapshot data
/// </summary>
public record BinanceMarginAccountSnapshotData
{
    /// <summary>
    /// The margin level
    /// </summary>
    public decimal MarginLevel { get; set; }

    /// <summary>
    /// Total BTC asset
    /// </summary>
    public decimal TotalAssetOfBtc { get; set; }

    /// <summary>
    /// Total BTC liability
    /// </summary>
    public decimal TotalLiabilityOfBtc { get; set; }

    /// <summary>
    /// Total net BTC asset
    /// </summary>
    public decimal TotalNetAssetOfBtc { get; set; }

    /// <summary>
    /// Assets
    /// </summary>
    public IEnumerable<BinanceMarginBalance> UserAssets { get; set; } = [];
}
