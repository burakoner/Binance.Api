namespace Binance.Api.Models.RestApi.Margin;

/// <summary>
/// Transfer history entry
/// </summary>
public record BinanceTransferHistory
{
    /// <summary>
    /// Quanity of the transfer
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Asset of the transfer
    /// </summary>
    public string Asset { get; set; } = "";
    /// <summary>
    /// Status of the transfer
    /// </summary>
    public string Status { get; set; } = "";
    /// <summary>
    /// Timestamp of the transaction
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("txId")]
    public decimal TransactionId { get; set; }
    /// <summary>
    /// Direction of the transfer
    /// </summary>
    [JsonProperty("type"), JsonConverter(typeof(TransferDirectionConverter))]
    public TransferDirection Direction { get; set; }
}
