namespace Binance.Api.Spot;

/// <summary>
/// Update when asset is withdrawn/deposited 
/// </summary>
public record BinanceSpotStreamBalanceUpdate: BinanceSpotStreamEvent
{
    /// <summary>
    /// The asset which changed
    /// </summary>
    [JsonProperty("a")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// The balance delta
    /// </summary>
    [JsonProperty("d")]
    public decimal BalanceDelta { get; set; }

    /// <summary>
    /// The time the deposit/withdrawal was cleared
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ClearTime { get; set; }

    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;
}
