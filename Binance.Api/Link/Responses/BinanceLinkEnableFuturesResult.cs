﻿namespace Binance.Api.Link;

/// <summary>
/// Enable Futures Result
/// </summary>
public record BinanceLinkEnableFuturesResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Is Futures Enabled
    /// </summary>
    [JsonProperty("enableFutures")]
    public bool IsFuturesEnabled { get; set; }

    /// <summary>
    /// Update Date
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateDate { get; set; }
}