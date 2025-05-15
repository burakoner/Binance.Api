namespace Binance.Api.Futures;

/// <summary>
/// Index info
/// </summary>
public record BinanceFuturesCompositeIndexInfo
{
    /// <summary>
    /// The symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Component asset
    /// </summary>
    [JsonProperty("component")]
    public string Component { get; set; } = string.Empty;

    /// <summary>
    /// Base asset list
    /// </summary>
    [JsonProperty("baseAssetList")]
    public List<BinanceFuturesCompositeIndexInfoAsset> BaseAssets { get; set; } = [];
}

/// <summary>
/// Composite index asset
/// </summary>
public record BinanceFuturesCompositeIndexInfoAsset
{
    /// <summary>
    /// Base asset name
    /// </summary>
    [JsonProperty("baseAsset")]
    public string BaseAsset { get; set; } = string.Empty;

    /// <summary>
    /// Quote asset name
    /// </summary>
    [JsonProperty("quoteAsset")]
    public string QuoteAsset { get; set; } = string.Empty;

    /// <summary>
    /// Weight in quantity
    /// </summary>
    [JsonProperty("weightInQuantity")]
    public decimal WeightInQuantity { get; set; }

    /// <summary>
    /// Weight in percentage
    /// </summary>
    [JsonProperty("weightInPercentage")]
    public decimal WeightInPercentage { get; set; }
}
