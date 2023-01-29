namespace Binance.ApiClient.Models.RestApi.CryptoLoans;

/// <summary>
/// Adjust info
/// </summary>
public class BinanceCryptoLoanLtvAdjust
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
    /// Current ltv
    /// </summary>
    public decimal CurrentLtv { get; set; }
}
