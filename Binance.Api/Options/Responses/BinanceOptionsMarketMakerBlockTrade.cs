namespace Binance.Api.Options;

/// <summary>
/// Options Market Maker Block Trade
/// </summary>
public record BinanceOptionsMarketMakerBlockTrade
{
    /// <summary>
    /// Block Trading Settlement Key
    /// </summary>
    public string BlockTradeSettlementKey { get; set; } = "";

    /// <summary>
    /// Parent Order ID of the block trade
    /// </summary>
    public string ParentOrderId { get; set; } = "";

    /// <summary>
    /// Cross Type of the block trade
    /// </summary>
    public string CrossType { get; set; } = "";

    /// <summary>
    /// Legs of the block trade
    /// </summary>
    public List<BinanceOptionsMarketMakerBlockTradeLeg> Legs { get; set; } = [];
}
