namespace Binance.Api.Options;

/// <summary>
/// Options Account Funding Flow
/// </summary>
public record BinanceOptionsAccountFundingFlow
{
    /// <summary>
    /// Unique identifier for the funding flow
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Asset type
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// Amount (positive numbers represent inflow, negative numbers represent outflow)
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// type (fees)
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateDate { get; set; }
}