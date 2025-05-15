namespace Binance.Api.Broker;

/// <summary>
/// BNB Burn Status
/// </summary>
public record BinanceBrokerBnbBurnStatus
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

    /// <summary>
    /// Is Interest BNB Burn
    /// </summary>
    [JsonProperty("interestBNBBurn")]
    public bool IsInterestBnbBurn { get; set; }
}