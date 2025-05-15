﻿namespace Binance.Api.Futures;

/// <summary>
/// Composite index info
/// </summary>
public record BinanceFuturesStreamCompositeIndex : BinanceFuturesStreamEvent
{
    /// <summary>
    /// The symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The price
    /// </summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>
    /// Base asset
    /// </summary>
    [JsonProperty("C")]
    public string BaseAsset { get; set; } = string.Empty;

    /// <summary>
    /// Composition
    /// </summary>
    [JsonProperty("c")]
    public List<BinanceFuturesStreamCompositeIndexAsset> Composition { get; set; } = [];
}

/// <summary>
/// Composite index asset info
/// </summary>
public record BinanceFuturesStreamCompositeIndexAsset
{
    /// <summary>
    /// Base asset name
    /// </summary>
    [JsonProperty("b")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Quote asset name
    /// </summary>
    [JsonProperty("q")]
    public string QuoteAsset { get; set; } = string.Empty;

    /// <summary>
    /// Weight in quantity
    /// </summary>
    [JsonProperty("w")]
    public decimal WeightInQuantity { get; set; }

    /// <summary>
    /// Weight in percentage
    /// </summary>
    [JsonProperty("W")]
    public decimal WeightInPercentage { get; set; }

    /// <summary>
    /// Index price
    /// </summary>
    [JsonProperty("i")]
    public decimal IndexPrice { get; set; }
}
