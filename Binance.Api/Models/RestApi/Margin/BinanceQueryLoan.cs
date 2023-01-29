﻿namespace Binance.ApiClient.Models.RestApi.Margin;

/// <summary>
/// Loan info
/// </summary>
public class BinanceLoan
{
    /// <summary>
    /// Isolated symbol
    /// </summary>
    public string IsolatedSymbol { get; set; }
    /// <summary>
    /// The asset of the loan
    /// </summary>
    public string Asset { get; set; }
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
    /// Time of repay completed
    /// </summary>
    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// The status of the loan
    /// </summary>
    [JsonConverter(typeof(MarginStatusConverter))]
    public MarginStatus Status { get; set; }
}
