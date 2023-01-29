﻿namespace Binance.ApiClient.Models.RestApi;

/// <summary>
/// Bnb burn status
/// </summary>
public class BinanceBnbBurnStatus
{
    /// <summary>
    /// If spot trading BNB burn is enabled
    /// </summary>
    public bool SpotBnbBurn { get; set; }
    /// <summary>
    /// If margin interest BNB burn is enabled
    /// </summary>
    public bool InterestBnbBurn { get; set; }
}
