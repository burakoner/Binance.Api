namespace Binance.Api.Broker;

/// <summary>
/// Spot Asset Info
/// </summary>
public record BinanceBrokerSpotAssetInfo
{
    /// <summary>
    /// Data
    /// </summary>
    [JsonProperty("data")]
    public List<BinanceBrokerageSubAccountSpotAssetInfo> Data { get; set; } = [];

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Account Spot Asset Info
/// </summary>
public record BinanceBrokerageSubAccountSpotAssetInfo
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Total Balance Of Btc
    /// </summary>
    [JsonProperty("totalBalanceOfBtc")]
    public decimal TotalBalanceOfBtc { get; set; }
}