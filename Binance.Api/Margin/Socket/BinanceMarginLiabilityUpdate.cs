namespace Binance.Api.Margin;

/// <summary>
/// Cross Margin liability update caused by borrowing, repayment, or interest calculation.
/// </summary>
public record BinanceMarginLiabilityUpdate : BinanceSocketStreamEvent
{
    /// <summary>
    /// Liability asset.
    /// </summary>
    [JsonProperty("a")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Liability update type reported by Binance.
    /// </summary>
    [JsonProperty("t")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Principal quantity.
    /// </summary>
    [JsonProperty("p")]
    public decimal PrincipalQuantity { get; set; }

    /// <summary>
    /// Interest quantity.
    /// </summary>
    [JsonProperty("i")]
    public decimal InterestQuantity { get; set; }
}
