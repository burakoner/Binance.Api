namespace Binance.Api.Margin;

/// <summary>
/// Transfer history entry
/// </summary>
public record BinanceMarginTransferHistory
{
    /// <summary>
    /// Quantity of the transfer
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
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Raw transaction timestamp. Binance does not define the unit for this field.
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("txId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// Direction of the transfer
    /// </summary>
    [JsonProperty("type")]
    [JsonConverter(typeof(MapConverter))]
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
    public string? FromSymbol { get; set; }

    /// <summary>
    /// Transfer to symbol
    /// </summary>
    public string? ToSymbol { get; set; }
}

/// <summary>
/// Paginated Margin transfer history.
/// </summary>
public record BinanceMarginTransferHistoryResult
{
    /// <summary>
    /// Transfer records in descending order.
    /// </summary>
    public List<BinanceMarginTransferHistory> Rows { get; set; } = [];

    /// <summary>
    /// Total number of records.
    /// </summary>
    public long Total { get; set; }
}
