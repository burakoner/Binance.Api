﻿namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Api Key Create Result
/// </summary>
public record BinanceBrokerageApiKeyCreateResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; } = "";

    /// <summary>
    /// Api Key
    /// </summary>
    public string ApiKey { get; set; } = "";

    /// <summary>
    /// Api Secret
    /// </summary>
    [JsonProperty("secretKey")]
    public string ApiSecret { get; set; } = "";

    /// <summary>
    /// Is Spot Trading Enabled
    /// </summary>
    [JsonProperty("canTrade")]
    public bool IsSpotTradingEnabled { get; set; }

    /// <summary>
    /// Is Margin Trading Enabled
    /// </summary>
    [JsonProperty("marginTrade")]
    public bool IsMarginTradingEnabled { get; set; }

    /// <summary>
    /// Is Futures Trading Enabled
    /// </summary>
    [JsonProperty("futuresTrade")]
    public bool IsFuturesTradingEnabled { get; set; }
}