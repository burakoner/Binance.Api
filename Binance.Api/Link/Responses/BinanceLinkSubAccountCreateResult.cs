﻿namespace Binance.Api.Link;

/// <summary>
/// Sub Account Create Result
/// </summary>
public record BinanceLinkSubAccountCreateResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Email
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Tag
    /// </summary>
    [JsonProperty("tag")]
    public string Tag { get; set; } = string.Empty;
}