namespace Binance.Api.Margin;

/// <summary>
/// Transfer history entry
/// </summary>
public record BinanceMarginTransferHistory
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
    [JsonProperty("type")]
    public BinanceMarginTransferDirection Direction { get; set; }

    /// <summary>
    /// Transfer from
    /// </summary>
    [JsonProperty("transFrom")]
    public string TransferFrom { get; set; } = string.Empty;

    /// <summary>
    /// Transfer to
    /// </summary>
    [JsonProperty("transTo")]
    public string TransferTo { get; set; } = string.Empty;

    /// <summary>
    /// Transfer from symbol
    /// </summary>
    public string? FromSymbol { get; set; } = string.Empty;

    /// <summary>
    /// Transfer to symbol
    /// </summary>
    public string? ToSymbol { get; set; } = string.Empty;
}
