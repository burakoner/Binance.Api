namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Block Trading Leg
/// </summary>
public record BinanceOptionsMarketMakerBlockOrderLeg
{
    /// <summary>
    /// Symbol of the leg in the block trade
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Order Side of the leg in the block trade
    /// </summary>
    [JsonProperty("side")]
    [JsonConverter(typeof(MapConverter))]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Price of the leg in the block trade
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity of the leg in the block trade
    /// </summary>
    [JsonProperty("quantity")]
    public decimal Quantity { get; set; }
}