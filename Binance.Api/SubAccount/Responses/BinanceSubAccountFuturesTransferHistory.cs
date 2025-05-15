namespace Binance.Api.SubAccount;

/// <summary>
/// Binance sub account asset transfer history
/// </summary>
public record BinanceSubAccountFuturesTransferHistory
{
    /// <summary>
    /// Success
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Futures Type
    /// </summary>
    [JsonProperty("futuresType")]
    public BinanceFuturesType FuturesType { get; set; }

    /// <summary>
    /// Transfers
    /// </summary>
    [JsonProperty("transfers")]
    public List<BinanceSubAccountAssetTransfer> Transfers { get; set; } = [];
}

/// <summary>
/// Binance sub account transfer
/// </summary>
public record BinanceSubAccountAssetTransfer
{
    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// From email
    /// </summary>
    [JsonProperty("from")]
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// To email
    /// </summary>
    [JsonProperty("to")]
    public string To { get; set; } = string.Empty;

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The time transaction was created
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }
}
