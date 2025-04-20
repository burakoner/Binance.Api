﻿namespace Binance.Api.Models.RestApi.SubAccount;

/// <summary>
/// Sub accounts futures summary
/// </summary>
public record BinanceSubAccountFuturesSummary
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = "";
    /// <summary>
    /// Total initial margin
    /// </summary>
    public decimal TotalInitialMargin { get; set; }
    /// <summary>
    /// Total maintenance margin
    /// </summary>
    public decimal TotalMaintenanceMargin { get; set; }
    /// <summary>
    /// Total margin balance
    /// </summary>
    public decimal TotalMarginBalance { get; set; }
    /// <summary>
    /// Total open order initial margin
    /// </summary>
    public decimal TotalOpenOrderInitialMargin { get; set; }
    /// <summary>
    /// Total position initial margin
    /// </summary>
    public decimal TotalPositionInitialMargin { get; set; }
    /// <summary>
    /// Total unrealized profit
    /// </summary>
    public decimal TotalUnrealizedProfit { get; set; }
    /// <summary>
    /// Total wallet balance
    /// </summary>
    public decimal TotalWalletBalance { get; set; }

    /// <summary>
    /// Sub accounts info
    /// </summary>
    [JsonProperty("subAccountList")]
    public IEnumerable<BinanceSubAccountFuturesInfo> SubAccounts { get; set; } = Array.Empty<BinanceSubAccountFuturesInfo>();
}

/// <summary>
/// Sub account future details
/// </summary>
public record BinanceSubAccountFuturesInfo
{
    /// <summary>
    /// Email of the sub account
    /// </summary>
    public string Email { get; set; } = "";
    /// <summary>
    /// Total initial margin
    /// </summary>
    public decimal TotalInitialMargin { get; set; }
    /// <summary>
    /// Total maintenance margin
    /// </summary>
    public decimal TotalMaintenanceMargin { get; set; }
    /// <summary>
    /// Total margin balance
    /// </summary>
    public decimal TotalMarginBalance { get; set; }
    /// <summary>
    /// Total open order initial margin
    /// </summary>
    public decimal TotalOpenOrderInitialMargin { get; set; }
    /// <summary>
    /// Total position initial margin
    /// </summary>
    public decimal TotalPositionInitialMargin { get; set; }
    /// <summary>
    /// Total unrealized profit
    /// </summary>
    public decimal TotalUnrealizedProfit { get; set; }
    /// <summary>
    /// Total wallet balance
    /// </summary>
    public decimal TotalWalletBalance { get; set; }
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = "";
}
