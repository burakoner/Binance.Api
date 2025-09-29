namespace Binance.Api.Wallet;

/// <summary>
/// Binance Wallet Delegation History
/// </summary>
public record BinanceWalletDelegationHistory
{
    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string TransactionId { get; set; } = "";

    /// <summary>
    /// Type
    /// </summary>
    [JsonProperty("transferType")]
    [JsonConverter(typeof(MapConverter))]
    public BinanceWalletDelegationTransferType Type { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Time
    /// </summary>
    [JsonProperty("time")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
}
