﻿namespace Binance.Api.Models.WebSocketApi.MarketData;

/// <summary>
/// Update when asset is withdrawn/deposited 
/// </summary>
public class BinanceStreamBalanceUpdate : BinanceSocketEvent
{
    /// <summary>
    /// The asset which changed
    /// </summary>
    [JsonProperty("a")]
    public string Asset { get; set; }
    /// <summary>
    /// The balance delta
    /// </summary>
    [JsonProperty("d")]
    public decimal BalanceDelta { get; set; }
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    public string ListenKey { get; set; }
    /// <summary>
    /// The time the deposit/withdrawal was cleared
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime ClearTime { get; set; }
}
