namespace Binance.Api.Options;

/// <summary>
/// Options User Trade Record
/// </summary>
public record BinanceOptionsUserTrade
{
    /// <summary>
    /// Unique Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Trade Id
    /// </summary>
    public long TradeId { get; set; }

    /// <summary>
    /// Order Id
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Price of the trade
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Fee
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Realized Profit
    /// </summary>
    public decimal RealizedProfit { get; set; }

    /// <summary>
    /// Order Side
    /// </summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Order Type
    /// </summary>
    public BinanceOptionsOrderType Type { get; set; }

    /// <summary>
    /// Volatility
    /// </summary>
    public decimal Volatility { get; set; }

    /// <summary>
    /// Liquidity
    /// </summary>
    public BinanceOptionsLiquidity Liquidity { get; set; }

    /// <summary>
    /// Timestamp of the trade
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    /// <summary>
    /// Price Scale
    /// </summary>
    public int PriceScale { get; set; }

    /// <summary>
    /// Quantity Scale
    /// </summary>
    public int QuantityScale { get; set; }

    /// <summary>
    /// Option Side
    /// </summary>
    [JsonProperty("optionSide")]
    public BinanceOptionsSide? OptionSide { get; set; }

    /// <summary>
    /// Quote Asset
    /// </summary>
    public string QuoteAsset { get; set; } = "";
}
