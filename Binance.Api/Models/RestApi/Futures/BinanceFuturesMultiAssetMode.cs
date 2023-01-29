﻿namespace Binance.ApiClient.Models.RestApi.Futures;

/// <summary>
/// Multi asset mode info
/// </summary>
public class BinanceFuturesMultiAssetMode
{
    /// <summary>
    /// Is multi assets mode enabled
    /// </summary>
    [JsonProperty("multiAssetsMargin")]
    public bool MultiAssetMode { get; set; }
}
