namespace Binance.Api.Options;

/// <summary>
/// Options symbol trading filter
/// </summary>
public record BinanceOptionsSymbolFilter
{
    /// <summary>
    /// Filter type
    /// </summary>
    public string FilterType { get; set; } = "";

    /// <summary>
    /// Minimum price
    /// </summary>
    public decimal MinPrice { get; set; }

    /// <summary>
    /// Maximum price
    /// </summary>
    public decimal MaxPrice { get; set; }

    /// <summary>
    /// Price increment
    /// </summary>
    public decimal TickSize { get; set; }

    /// <summary>
    /// Minimum quantity
    /// </summary>
    [JsonProperty("minQty")]
    public decimal MinQuantity { get; set; }

    /// <summary>
    /// Maximum quantity
    /// </summary>
    [JsonProperty("maxQty")]
    public decimal MaxQuantity { get; set; }

    /// <summary>
    /// Quantity increment
    /// </summary>
    public decimal StepSize { get; set; }
}
