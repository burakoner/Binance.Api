namespace Binance.Api.SubAccount;

/// <summary>
/// Information about an asset balance
/// </summary>
public class BinanceSubAccountBalance
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
    /// Frozen
    /// </summary>
    [JsonProperty("freeze")]
    public decimal Frozen { get; set; }

    /// <summary>
    /// The quantity that is currently being withdrawn
    /// </summary>
    public decimal Withdrawing { get; set; }
}