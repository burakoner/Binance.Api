﻿namespace Binance.Api.Wallet;

/// <summary>
/// Dividend record
/// </summary>
public record BinanceDividendRecord
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// Timestamp of the transaction
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter)), JsonProperty("divTime")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("tranId")]
    public string TransactionId { get; set; } = "";

    /// <summary>
    /// Info
    /// </summary>
    [JsonProperty("enInfo")]
    public string Info { get; set; } = "";
}
