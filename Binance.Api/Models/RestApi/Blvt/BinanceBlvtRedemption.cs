namespace Binance.ApiClient.Models.RestApi.Blvt;

/// <summary>
/// Redemption info
/// </summary>
public class BinanceBlvtRedemption
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Token name
    /// </summary>
    public string TokenName { get; set; }
    /// <summary>
    /// Redemption quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// NAV price of redemption
    /// </summary>
    public decimal Nav { get; set; }
    /// <summary>
    /// Redemption fee in usdt
    /// </summary>
    public decimal Fee { get; set; }
    /// <summary>
    /// Net redemption value in usdt
    /// </summary>
    public decimal NetProceed { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
