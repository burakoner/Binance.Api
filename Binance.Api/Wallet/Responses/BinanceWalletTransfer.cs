namespace Binance.Api.Wallet;

/// <summary>
/// Transfer info
/// </summary>
public record BinanceWalletTransfer
{
    /// <summary>
    /// The asset which was transfered
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// Quantity transfered
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Transfer type
    /// </summary>
    public BinanceWalletUniversalTransferType Type { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; } = "";

    /// <summary>
    /// Id
    /// </summary>
    [JsonProperty("tranId")]
    public long Id { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
