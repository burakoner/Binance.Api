namespace Binance.Api.Models.RestApi.SubAccount;

internal class BinanceSubAccountTransferWrapper
{
    [JsonProperty("msg")]
    public string? Message { get; set; }
    public bool Success { get; set; }
    public IEnumerable<BinanceSubAccountTransfer> Transfers { get; set; }
}

/// <summary>
/// Sub account transfer info
/// </summary>
public record BinanceSubAccountTransfer
{
    /// <summary>
    /// From which email the transfer originated
    /// </summary>
    public string From { get; set; } = "";
    /// <summary>
    /// To which email the transfer was to
    /// </summary>
    public string To { get; set; } = "";
    /// <summary>
    /// The asset of the transfer
    /// </summary>
    public string Asset { get; set; } = "";
    /// <summary>
    /// The quantity of the transfer
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// The timestamp of the transfer
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Status of the transaction
    /// </summary>
    public string Status { get; set; } = "";
    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }
}
