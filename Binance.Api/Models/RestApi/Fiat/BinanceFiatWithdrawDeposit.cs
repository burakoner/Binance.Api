namespace Binance.Api.Models.RestApi.Fiat;

/// <summary>
/// Fiat payment info
/// </summary>
public class BinanceFiatWithdrawDeposit
{
    /// <summary>
    /// Order number
    /// </summary>
    [JsonProperty("orderNo")]
    public string OrderNumber { get; set; }
    /// <summary>
    /// The used asset
    /// </summary>
    [JsonProperty("fiatCurrency")]
    public string FiatAsset { get; set; }
    /// <summary>
    /// The quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// The indicated quantity
    /// </summary>
    [JsonProperty("indicatedAmount")]
    public decimal IndicatedQuantity { get; set; }
    /// <summary>
    /// The method
    /// </summary>
    public string Method { get; set; }
    /// <summary>
    /// The total fee of the order
    /// </summary>
    public decimal TotalFee { get; set; }
    /// <summary>
    /// The status 
    /// </summary>
    [JsonConverter(typeof(FiatWithdrawDepositStatusConverter))]
    public FiatWithdrawDepositStatus Status { get; set; }
    /// <summary>
    /// Creation time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// Last update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
}
