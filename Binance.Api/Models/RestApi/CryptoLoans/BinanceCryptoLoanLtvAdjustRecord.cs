namespace Binance.Api.Models.RestApi.CryptoLoans;

/// <summary>
/// Ltv adjustment info
/// </summary>
public class BinanceCryptoLoanLtvAdjustRecord
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; }
    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; }
    /// <summary>
    /// Direction
    /// </summary>
    public string Direction { get; set; }
    /// <summary>
    /// Amount
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Pre adjust ltv
    /// </summary>
    public decimal PreLtv { get; set; }
    /// <summary>
    /// Post adjust ltv
    /// </summary>
    public decimal AfterLtv { get; set; }
    /// <summary>
    /// Adjust time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime AdjustTime { get; set; }
    /// <summary>
    /// Order id
    /// </summary>
    public long OrderId { get; set; }
}
