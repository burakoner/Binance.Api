namespace Binance.Api.Wallet;

/// <summary>
/// Snapshot data of a spot account
/// </summary>
public record BinanceWalletSpotAccountSnapshot
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
    public BinanceSpotAccountSnapshotData Data { get; set; } = default!;
}

/// <summary>
/// Data of the snapshot
/// </summary>
public record BinanceSpotAccountSnapshotData
{
    /// <summary>
    /// The total value of assets in btc
    /// </summary>
    public decimal TotalAssetOfBtc { get; set; }

    /// <summary>
    /// List of balances
    /// </summary>
    public List<BinanceWalletSpotAccountBalance> Balances { get; set; } = [];
}
