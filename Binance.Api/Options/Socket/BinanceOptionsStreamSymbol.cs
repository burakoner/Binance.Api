namespace Binance.Api.Options;

/// <summary>
/// Binance Options Web Socket Stream New Symbol Information
/// </summary>
public record BinanceOptionsStreamSymbol : BinanceSocketStreamEvent
{
    /// <summary>
    /// Underlying
    /// </summary>
    [JsonProperty("u")]
    public string Underlying { get; set; } = "";

    /// <summary>
    /// Quotation Asset
    /// </summary>
    [JsonProperty("qa")]
    public string QuoteAsset { get; set; } = "";

    /// <summary>
    /// Trading pair name
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Conversion ratio, the quantity of the underlying asset represented by a single contract. 
    /// </summary>
    [JsonProperty("unit")]
    public decimal Unit { get; set; }

    /// <summary>
    /// Minimum trade volume of the underlying asset 
    /// </summary>
    [JsonProperty("mq")]
    public decimal MinimumTradeVolume { get; set; }

    /// <summary>
    /// Option type
    /// </summary>
    [JsonProperty("d")]
    public BinanceOptionsSide Side { get; set; }

    /// <summary>
    /// Strike Price
    /// </summary>
    [JsonProperty("sp")]
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// expiration time  
    /// </summary>
    [JsonProperty("ed"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TradeTime { get; set; }
}
