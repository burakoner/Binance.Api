namespace Binance.Api.Link;

/// <summary>
/// Enable Or Disable BNB Burn Margin Interest Result
/// </summary>
public record BinanceLinkChangeBnbBurnMarginInterestResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Is Interest BNB Burn
    /// </summary> 
    [JsonProperty("interestBNBBurn")]
    public bool IsInterestBnbBurn { get; set; }
}