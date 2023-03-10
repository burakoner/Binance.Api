namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// Funding rate information for Futures trading
/// </summary>
public class BinanceFuturesFundingRateHistory
{
    /// <summary>
    /// The symbol the information is about
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// The finding rate for the given symbol and time
    /// </summary>
    public decimal FundingRate { get; set; }
    /// <summary>
    /// The time the funding rate is applied
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime FundingTime { get; set; }
}
