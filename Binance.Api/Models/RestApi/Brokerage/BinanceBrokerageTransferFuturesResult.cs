﻿namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Transfer Futures Result
/// </summary>
public class BinanceBrokerageTransferFuturesResult
{
    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txnId")]
    public string Id { get; set; }

    /// <summary>
    /// Success
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Client Transfer Id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransferId { get; set; }
}