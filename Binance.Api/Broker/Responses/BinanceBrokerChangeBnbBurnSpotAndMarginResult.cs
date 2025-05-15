namespace Binance.Api.Broker;

/// <summary>
/// Enable Or Disable BNB Burn Spot And Margin Result
/// </summary>
public record BinanceBrokerChangeBnbBurnSpotAndMarginResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Is Spot BNB Burn
    /// </summary>
    [JsonProperty("spotBNBBurn")]
    public bool IsSpotBnbBurn { get; set; }
}