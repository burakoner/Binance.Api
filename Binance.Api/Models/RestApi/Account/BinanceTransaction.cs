﻿namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// The result of transferring
/// </summary>
public class BinanceTransaction
{
    /// <summary>
    /// The Transaction id as assigned by Binance
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }
}
