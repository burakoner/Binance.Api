namespace Binance.Api.Wallet;

/// <summary>
/// Spot symbol delist info
/// </summary>
public record BinanceWalletDelistSchedule
{
    /// <summary>
    /// Delist time
    /// </summary>
    [JsonProperty("delistTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime DelistTime { get; set; }

    /// <summary>
    /// Symbols being delisted
    /// </summary>
    [JsonProperty("symbols")]
    public IEnumerable<string> Symbols { get; set; } = [];
}
