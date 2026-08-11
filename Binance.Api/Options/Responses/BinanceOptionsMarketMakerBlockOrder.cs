namespace Binance.Api.Options;

/// <summary>
/// Options Market Maker Block Trading Order
/// </summary>
public record BinanceOptionsMarketMakerBlockOrder
{
    /// <summary>
    /// Block Trading Settlement Key
    /// </summary>
    public string BlockTradeSettlementKey { get; set; } = "";

    /// <summary>
    /// Expire Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// Liquidity
    /// </summary>
    public BinanceOptionsLiquidity Liquidity { get; set; }

    /// <summary>
    /// Order Status
    /// </summary>
    public string Status { get; set; } = "";

    /// <summary>
    /// Creation time. Not returned by the new block trade order endpoint.
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// Legs of the block order
    /// </summary>
    public List<BinanceOptionsMarketMakerBlockOrderLeg> Legs { get; set; } = [];
}
