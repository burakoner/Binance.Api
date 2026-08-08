namespace Binance.Api.Spot;

/// <summary>
/// Update when part of a Spot wallet balance is locked or unlocked by an external system.
/// </summary>
public record BinanceSpotStreamExternalLockUpdate : BinanceSpotUserDataStreamEvent
{
    /// <summary>
    /// Asset whose external lock changed.
    /// </summary>
    [JsonProperty("a")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Change in the externally locked quantity.
    /// </summary>
    [JsonProperty("d")]
    public decimal Delta { get; set; }

    /// <summary>
    /// Transaction time.
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }
}
