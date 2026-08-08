namespace Binance.Api.Margin;

/// <summary>
/// Margin balance update caused by a deposit, withdrawal, or transfer.
/// </summary>
public record BinanceMarginStreamBalanceUpdate : BinanceMarginStreamUpdate
{
    /// <summary>Changed asset.</summary>
    [JsonProperty("a")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>Balance delta.</summary>
    [JsonProperty("d")]
    public decimal BalanceDelta { get; set; }

    /// <summary>Clear time.</summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ClearTime { get; set; }
}
