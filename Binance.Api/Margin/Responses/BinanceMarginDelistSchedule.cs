namespace Binance.Api.Margin;

/// <summary>
/// Delist margin schedule
/// </summary>
public record BinanceMarginDelistSchedule
{
    /// <summary>
    /// Delist time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime DelistTime { get; set; }

    /// <summary>
    /// Cross margin assets
    /// </summary>
    public List<string> CrossMarginAssets { get; set; } = [];

    /// <summary>
    /// Isolated margin symbols
    /// </summary>
    public List<string> IsolatedMarginSymbols { get; set; } = [];
}
