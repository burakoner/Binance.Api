﻿namespace Binance.Api.Margin;

/// <summary>
/// Loan info
/// </summary>
public record BinanceLoan
{
    /// <summary>
    /// Isolated symbol
    /// </summary>
    public string IsolatedSymbol { get; set; } = "";

    /// <summary>
    /// The asset of the loan
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// The transaction id of the loan
    /// </summary>
    [JsonProperty("txId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// Principal repaid 
    /// </summary>
    public decimal Principal { get; set; }

    /// <summary>
    /// Interest repaid 
    /// </summary>
    public decimal Interest { get; set; }

    /// <summary>
    /// Quantity repaid 
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Time of repay completed
    /// </summary>
    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The status of the loan
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceMarginStatus Status { get; set; }
}
