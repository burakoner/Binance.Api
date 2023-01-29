﻿namespace Binance.Api.Models.RestApi.Blvt;

/// <summary>
/// Redeem result
/// </summary>
public class BinanceBlvtRedeemResult
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Status
    /// </summary>
    [JsonConverter(typeof(BlvtStatusConverter))]
    public BlvtStatus Status { get; set; }
    /// <summary>
    /// Name of the token
    /// </summary>
    public string TokenName { get; set; }
    /// <summary>
    /// Redemption value in usdt
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Redemption token quantity
    /// </summary>
    [JsonProperty("redeemAmount")]
    public decimal RedeemQuantity { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
