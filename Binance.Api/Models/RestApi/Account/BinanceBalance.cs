namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// Information about an asset balance
/// </summary>
public record BinanceBalance : IBinanceBalance
{
    /// <summary>
    /// The asset this balance is for
    /// </summary>
    public string Asset { get; set; } = "";
    /// <summary>
    /// The quantity that isn't locked in a trade
    /// </summary>
    [JsonProperty("free")]
    public decimal Available { get; set; }
    /// <summary>
    /// The quantity that is currently locked in a trade
    /// </summary>
    public decimal Locked { get; set; }
    /// <summary>
    /// The total balance of this asset (Free + Locked)
    /// </summary>
    public decimal Total => Available + Locked;
}
