namespace Binance.Api.AutoInvest;

/// <summary>
/// Auto invest ROI
/// </summary>
public record BinanceAutoInvestRoi
{
    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Simulate roi
    /// </summary>
    [JsonProperty("simulateRoi")]
    public decimal SimulateRoi { get; set; }
}
