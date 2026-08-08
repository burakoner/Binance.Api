namespace Binance.Api.Margin;

/// <summary>
/// Margin balance included in an account-position update.
/// </summary>
public record BinanceMarginStreamBalance
{
    /// <summary>Asset.</summary>
    [JsonProperty("a")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>Free quantity.</summary>
    [JsonProperty("f")]
    public decimal Free { get; set; }

    /// <summary>Locked quantity.</summary>
    [JsonProperty("l")]
    public decimal Locked { get; set; }
}
