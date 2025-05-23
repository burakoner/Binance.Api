﻿namespace Binance.Api.Futures;

/// <summary>
/// Response to the change in initial leverage request
/// </summary>
public record BinanceFuturesInitialLeverageChangeResult
{
    /// <summary>
    /// New leverage multiplier
    /// </summary>
    [JsonProperty("leverage")]
    public int Leverage { get; set; }

    /// <summary>
    /// Maximum value that can be held
    /// NOTE: string type, because the value van be 'inf' (infinite)
    /// </summary>
    [JsonProperty("maxNotionalValue")]
    public string? MaxNotionalValue { get; set; }

    /// <summary>
    /// Max quantity
    /// </summary>
    [JsonProperty("maxQty")]
    public string? MaxQuantity { get; set; }

    /// <summary>
    /// Symbol the request is for
    /// </summary>
    [JsonProperty("symbol")]
    public string? Symbol { get; set; }
}
