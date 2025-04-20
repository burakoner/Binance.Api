﻿namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Spot Asset Info
/// </summary>
public record BinanceBrokerageSpotAssetInfo
{
    /// <summary>
    /// Data
    /// </summary>
    public IEnumerable<BinanceBrokerageSubAccountSpotAssetInfo> Data { get; set; } = Array.Empty<BinanceBrokerageSubAccountSpotAssetInfo>();

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
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
    public string SubAccountId { get; set; } = "";

    /// <summary>
    /// Total Balance Of Btc
    /// </summary>
    public decimal TotalBalanceOfBtc { get; set; }
}