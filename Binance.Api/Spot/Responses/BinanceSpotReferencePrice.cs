namespace Binance.Api.Spot;

/// <summary>
/// Current reference price for a Spot symbol.
/// </summary>
public record BinanceSpotReferencePrice
{
    /// <summary>
    /// Symbol.
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Reference price, or <see langword="null"/> when no reference price is set.
    /// </summary>
    public decimal? ReferencePrice { get; set; }

    /// <summary>
    /// Time at which the reference price was valid.
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Configuration used to calculate a Spot reference price.
/// </summary>
public record BinanceSpotReferencePriceCalculation
{
    /// <summary>
    /// Symbol.
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Calculation method.
    /// </summary>
    public BinanceSpotReferencePriceCalculationType CalculationType { get; set; }

    /// <summary>
    /// Number of buckets used by an arithmetic-mean calculation.
    /// </summary>
    public int? BucketCount { get; set; }

    /// <summary>
    /// Width of each arithmetic-mean bucket, in milliseconds.
    /// </summary>
    public long? BucketWidthMs { get; set; }

    /// <summary>
    /// External calculation identifier when calculation occurs outside the matching engine.
    /// </summary>
    public long? ExternalCalculationId { get; set; }
}
