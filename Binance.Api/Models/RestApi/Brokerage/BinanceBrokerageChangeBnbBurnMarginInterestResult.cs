﻿namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Enable Or Disable BNB Burn Margin Interest Result
/// </summary>
public class BinanceBrokerageChangeBnbBurnMarginInterestResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; }

    /// <summary>
    /// Is Interest BNB Burn
    /// </summary> 
    [JsonProperty("interestBNBBurn")]
    public bool IsInterestBnbBurn { get; set; }
}