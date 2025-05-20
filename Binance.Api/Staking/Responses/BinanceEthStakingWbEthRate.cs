namespace Binance.Api.Staking;

/// <summary>
/// Rate history
/// </summary>
public record BinanceEthStakingWbEthRate
{
    /// <summary>
    /// Exchange rate
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// Annual percentage rate
    /// </summary>
    public decimal AnnualPercentageRate { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }
}
