﻿namespace Binance.Api.Models.RestApi.Margin;

/// <summary>
/// Repay info
/// </summary>
public record BinanceRepay
{
    /// <summary>
    /// Isolated symbol
    /// </summary>
    public string IsolatedSymbol { get; set; } = "";
    /// <summary>
    /// The asset of the repay
    /// </summary>
    public string Asset { get; set; } = "";
    /// <summary>
    /// The transaction id of the repay
    /// </summary>`
    [JsonProperty("txId")]
    public long TransactionId { get; set; }
    /// <summary>
    /// Total quantity repaid
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Interest repaid
    /// </summary>
    public decimal Interest { get; set; }
    /// <summary>
    /// Principal repaid
    /// </summary>
    public decimal Principal { get; set; }
    /// <summary>
    /// Time of repay completed
    /// </summary>
    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// The status of the repay
    /// </summary>
    [JsonProperty("status"), JsonConverter(typeof(MarginStatusConverter))]
    public MarginStatus Status { get; set; }
}
