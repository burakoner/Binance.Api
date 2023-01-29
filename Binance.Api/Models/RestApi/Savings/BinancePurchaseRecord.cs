namespace Binance.Api.Models.RestApi.Savings;

/// <summary>
/// Purchase record
/// </summary>
public class BinancePurchaseRecord
{
    /// <summary>
    /// Quantity purchased
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Asset name
    /// </summary>
    public string Asset { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// Lending type
    /// </summary>
    [JsonConverter(typeof(LendingTypeConverter))]
    public LendingType LendingType { get; set; }
    /// <summary>
    /// Lot
    /// </summary>
    public int Lot { get; set; }
    /// <summary>
    /// Name of the product
    /// </summary>
    public string ProductName { get; set; }
    /// <summary>
    /// Purchase id
    /// </summary>
    public string PurchaseId { get; set; }

    /// <summary>
    /// Purchase status
    /// </summary>
    public string Status { get; set; }
}
