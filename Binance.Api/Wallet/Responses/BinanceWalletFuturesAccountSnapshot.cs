﻿namespace Binance.Api.Wallet;

/// <summary>
/// Snapshot data of a futures account
/// </summary>
public record BinanceWalletFuturesAccountSnapshot
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
    public BinanceFuturesAccountSnapshotData Data { get; set; } = default!;
}

/// <summary>
/// Data of the snapshot
/// </summary>
public record BinanceFuturesAccountSnapshotData
{
    /// <summary>
    /// List of assets
    /// </summary>
    public List<BinanceFuturesAsset> Assets { get; set; } = [];

    /// <summary>
    /// List of positions
    /// </summary>
    public List<BinanceFuturesSnapshotPosition> Position { get; set; } = [];
}

/// <summary>
/// Asset
/// </summary>
public record BinanceFuturesAsset
{
    /// <summary>
    /// Name of the asset
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// Margin balance
    /// </summary>
    public decimal MarginBalance { get; set; }

    /// <summary>
    /// Wallet balance
    /// </summary>
    public decimal? WalletBalance { get; set; }
}

/// <summary>
/// Position
/// </summary>
public record BinanceFuturesSnapshotPosition
{
    /// <summary>
    /// The symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Entry price
    /// </summary>
    public decimal EntryPrice { get; set; }

    /// <summary>
    /// Mark price
    /// </summary>
    public decimal? MarkPrice { get; set; }

    /// <summary>
    /// PositionAmt
    /// </summary>
    public decimal? PositionAmt { get; set; }

    /// <summary>
    /// Unrealized profit
    /// </summary>
    public decimal? UnrealizedProfit { get; set; }
}
