﻿namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple Earn result
/// </summary>
public record BinanceSimpleEarnResult
{
    /// <summary>
    /// Result
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }
}
