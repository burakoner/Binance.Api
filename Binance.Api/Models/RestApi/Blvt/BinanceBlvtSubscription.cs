﻿namespace Binance.Api.Models.RestApi.Blvt;

/// <summary>
/// Leveraged token subscription info
/// </summary>
public record BinanceBlvtSubscription
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Token name
    /// </summary>
    public string TokenName { get; set; } = "";
    /// <summary>
    /// Subscription quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// NAV price of subscription
    /// </summary>
    public decimal Nav { get; set; }
    /// <summary>
    /// Subscription fee in usdt
    /// </summary>
    public decimal Fee { get; set; }
    /// <summary>
    /// Subscription cost in usdt
    /// </summary>
    public decimal TotalCharge { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
