namespace Binance.Api.Models.RestApi.Savings;

/// <summary>
/// Interest record
/// </summary>
public record BinanceLendingInterestHistory
{
    /// <summary>
    /// Interest
    /// </summary>
    public decimal Interest { get; set; }
    /// <summary>
    /// Asset name
    /// </summary>
    public string Asset { get; set; } = "";
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Lending type
    /// </summary>
    [JsonConverter(typeof(LendingTypeConverter))]
    public LendingType LendingType { get; set; }
    /// <summary>
    /// Name of the product
    /// </summary>
    public string ProductName { get; set; } = "";
}
