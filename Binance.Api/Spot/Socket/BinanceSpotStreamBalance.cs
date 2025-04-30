namespace Binance.Api.Spot;

/// <summary>
/// Information about an asset balance
/// </summary>
public record BinanceSpotStreamBalance
{
    /// <summary>
    /// The asset this balance is for
    /// </summary>
    [JsonProperty("a")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// The quantity that isn't locked in a trade
    /// </summary>
    [JsonProperty("f")]
    public decimal Available { get; set; }

    /// <summary>
    /// The quantity that is currently locked in a trade
    /// </summary>
    [JsonProperty("l")]
    public decimal Locked { get; set; }

    /// <summary>
    /// The total balance of this asset (Free + Locked)
    /// </summary>
    public decimal Total => Available + Locked;
}
