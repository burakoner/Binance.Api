namespace Binance.ApiClient.Models.RestApi.Margin;

/// <summary>
/// Isolated margin transfer
/// </summary>
public class BinanceIsolatedMarginTransfer
{
    /// <summary>
    /// Quantity of the transfer
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Transfer asset
    /// </summary>
    public string Asset { get; set; }

    /// <summary>
    /// Status of the transfer
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// Timestamp of the transfer
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("txId")]
    public string TransactionId { get; set; }
    /// <summary>
    /// From
    /// </summary>
    [JsonProperty("transFrom"), JsonConverter(typeof(IsolatedMarginTransferDirectionConverter))]
    public IsolatedMarginTransferDirection From { get; set; }
    /// <summary>
    /// To
    /// </summary>
    [JsonProperty("transTo"), JsonConverter(typeof(IsolatedMarginTransferDirectionConverter))]
    public IsolatedMarginTransferDirection To { get; set; }
}
