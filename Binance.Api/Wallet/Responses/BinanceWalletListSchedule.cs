namespace Binance.Api.Wallet;

/// <summary>
/// Binance Wallet List Schedule
/// </summary>
public record BinanceWalletListSchedule
{
    /// <summary>
    /// Listing time
    /// </summary>
    [JsonProperty("openTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ListingTime { get; set; }

    /// <summary>
    /// Symbols being listed
    /// </summary>
    [JsonProperty("symbols")]
    public List<string> Symbols { get; set; } = [];
}
