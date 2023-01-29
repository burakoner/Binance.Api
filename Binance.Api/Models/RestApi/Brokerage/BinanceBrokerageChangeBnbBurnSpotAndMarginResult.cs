namespace Binance.ApiClient.Models.RestApi.Brokerage;

/// <summary>
/// Enable Or Disable BNB Burn Spot And Margin Result
/// </summary>
public class BinanceBrokerageChangeBnbBurnSpotAndMarginResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; }

    /// <summary>
    /// Is Spot BNB Burn
    /// </summary>
    [JsonProperty("spotBNBBurn")]
    public bool IsSpotBnbBurn { get; set; }
}