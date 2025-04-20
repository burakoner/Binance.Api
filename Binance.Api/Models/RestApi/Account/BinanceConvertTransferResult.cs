﻿namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// Result of a convert transfer operation
/// </summary>
public record BinanceConvertTransferResult
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
}
