namespace Binance.Api.Margin;

/// <summary>
/// Information about an asset balance
/// </summary>
public record BinanceMarginAccountBalance
{
    /// <summary>
    /// The asset this balance is for
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// The quantity that was borrowed
    /// </summary>
    public decimal Borrowed { get; set; }

    /// <summary>
    /// The quantity that isn't locked in a trade
    /// </summary>
    [JsonProperty("free")]
    public decimal Available { get; set; }

    /// <summary>
    /// Fee to need pay by borrowed
    /// </summary>
    public decimal Interest { get; set; }

    /// <summary>
    /// The quantity that is currently locked in a trade
    /// </summary>
    public decimal Locked { get; set; }

    /// <summary>
    /// The quantity that is netAsset
    /// </summary>
    public decimal NetAsset { get; set; }

    /// <summary>
    /// The total balance of this asset (Available + Locked)
    /// </summary>
    public decimal Total => Available + Locked;
}
