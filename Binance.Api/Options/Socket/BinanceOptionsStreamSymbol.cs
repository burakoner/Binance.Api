namespace Binance.Api.Options;

/// <summary>
/// Binance Options Web Socket Stream New Symbol Information
/// </summary>
public record BinanceOptionsStreamSymbol : BinanceSocketStreamEvent
{
    /// <summary>
    /// Underlying
    /// </summary>
    [JsonProperty("ps")]
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
    /// Contract unit, the quantity of the underlying asset represented by a single contract
    /// </summary>
    [JsonProperty("u")]
    public long Unit { get; set; }

    /// <summary>
    /// Option type
    /// </summary>
    [JsonProperty("d"), JsonConverter(typeof(MapConverter))]
    public BinanceOptionsSide Side { get; set; }

    /// <summary>
    /// Strike Price
    /// </summary>
    [JsonProperty("sp")]
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// Delivery date and time
    /// </summary>
    [JsonProperty("dt"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime DeliveryTime { get; set; }

    /// <summary>
    /// Onboard date and time
    /// </summary>
    [JsonProperty("ot"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OnboardTime { get; set; }

    /// <summary>
    /// Contract status
    /// </summary>
    [JsonProperty("cs")]
    public string Status { get; set; } = "";
}
