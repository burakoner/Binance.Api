﻿namespace Binance.Api.Models.RestApi.Margin;

/// <summary>
/// Interest rate history
/// </summary>
public class BinanceInterestRateHistory
{
    /// <summary>
    /// The asset
    /// </summary>
    public string Asset { get; set; }
    /// <summary>
    /// The daily interest
    /// </summary>
    [JsonProperty("dailyInterestRate")]
    public decimal DailyInterest { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Vip level
    /// </summary>
    public string VipLevel { get; set; }
}
