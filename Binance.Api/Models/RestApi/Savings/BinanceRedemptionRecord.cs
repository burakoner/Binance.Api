﻿namespace Binance.ApiClient.Models.RestApi.Savings;

/// <summary>
/// Redemption record
/// </summary>
public class BinanceRedemptionRecord
{
    /// <summary>
    /// Quantity purchased
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Asset name
    /// </summary>
    public string Asset { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// Start time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? StartTime { get; set; }
    /// <summary>
    /// Interest
    /// </summary>
    public decimal Interest { get; set; }
    /// <summary>
    /// Redeem type
    /// </summary>
    [JsonConverter(typeof(RedeemTypeConverter))]
    public RedeemType Type { get; set; }
    /// <summary>
    /// Id of the project
    /// </summary>
    public string ProjectId { get; set; }
    /// <summary>
    /// Name of the project
    /// </summary>
    public string ProjectName { get; set; }
    /// <summary>
    /// Principal
    /// </summary>
    public decimal Principal { get; set; }

    /// <summary>
    /// Purchase status
    /// </summary>
    public string Status { get; set; }
}
