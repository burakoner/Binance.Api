namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// BNB Burn Status
/// </summary>
public class BinanceBrokerageBnbBurnStatus
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

    /// <summary>
    /// Is Interest BNB Burn
    /// </summary>
    [JsonProperty("interestBNBBurn")]
    public bool IsInterestBnbBurn { get; set; }
}