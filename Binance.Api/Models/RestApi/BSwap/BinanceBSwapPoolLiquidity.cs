﻿namespace Binance.Api.Models.RestApi.BSwap;

/// <summary>
/// Pool liquidity info
/// </summary>
public class BinanceBSwapPoolLiquidity
{
    /// <summary>
    /// Id
    /// </summary>
    public int PoolId { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string PoolName { get; set; }
    /// <summary>
    /// Update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// Liquidity
    /// </summary>
    public Dictionary<string, decimal> Liquidity { get; set; } = new Dictionary<string, decimal>();
    /// <summary>
    /// Share
    /// </summary>
    public BinancePoolShare Share { get; set; } = new BinancePoolShare();
}

/// <summary>
/// Pool share info
/// </summary>
public class BinancePoolShare
{
    /// <summary>
    /// Share quantity
    /// </summary>
    [JsonProperty("shareAmount")]
    public decimal ShareQuantity { get; set; }
    /// <summary>
    /// Share percentage
    /// </summary>
    public decimal SharePercentage { get; set; }
    /// <summary>
    /// Asset
    /// </summary>
    public Dictionary<string, decimal> Asset { get; set; } = new Dictionary<string, decimal>();
}
