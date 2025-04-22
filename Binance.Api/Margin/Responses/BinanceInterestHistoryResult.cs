namespace Binance.Api.Margin.Responses;

/// <summary>
/// Interest history entry info
/// </summary>
public record BinanceInterestHistory
{
    /// <summary>
    /// Isolated symbol
    /// </summary>
    public string IsolatedSymbol { get; set; } = "";
    /// <summary>
    /// The asset
    /// </summary>
    public string Asset { get; set; } = "";
    /// <summary>
    /// The quantity of interest
    /// </summary>
    [JsonProperty("interest")]
    public decimal InterestQuantity { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime InterestAccuredTime { get; set; }
    /// <summary>
    /// Interest rate
    /// </summary>
    public decimal InterestRate { get; set; }
    /// <summary>
    /// Principal
    /// </summary>
    public decimal Principal { get; set; }
    /// <summary>
    /// Type of interest
    /// </summary>
    public string Type { get; set; } = "";
}
