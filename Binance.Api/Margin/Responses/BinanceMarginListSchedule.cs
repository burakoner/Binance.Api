namespace Binance.Api.Margin;

/// <summary>Upcoming Cross and Isolated Margin listing schedule entry.</summary>
public record BinanceMarginListSchedule
{
    /// <summary>Listing time.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ListTime { get; set; }

    /// <summary>Cross Margin assets to be listed.</summary>
    public List<string> CrossMarginAssets { get; set; } = [];

    /// <summary>Isolated Margin symbols to be listed.</summary>
    public List<string> IsolatedMarginSymbols { get; set; } = [];
}
