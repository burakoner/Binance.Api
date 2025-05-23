﻿namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple Earn rate record
/// </summary>
public record BinanceSimpleEarnFlexibleRateRecord
{
    /// <summary>
    /// Product id
    /// </summary>
    [JsonProperty("productId")]
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Annual percentage rate
    /// </summary>
    [JsonProperty("annualPercentageRate")]
    public decimal AnnualPercentageRate { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
