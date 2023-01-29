namespace Binance.ApiClient.Models.RestApi;

/// <summary>
/// Info on a product
/// </summary>
public class BinanceProduct
{
    /// <summary>
    /// Name of the symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; }

    /// <summary>
    /// Status of the symbol
    /// </summary>
    [JsonProperty("st")]
    public string Status { get; set; }

    /// <summary>
    /// Name of the base asset
    /// </summary>
    [JsonProperty("b")]
    public string BaseAsset { get; set; }
    /// <summary>
    /// Name of the quote asset
    /// </summary>
    [JsonProperty("q")]
    public string QuoteAsset { get; set; }

    /// <summary>
    /// Char of the base asset
    /// </summary>
    [JsonProperty("ba")]
    public string BaseAssetChar { get; set; }
    /// <summary>
    /// Char of the quote asset
    /// </summary>
    [JsonProperty("qa")]
    public string QuoteAssetChar { get; set; }

    /// <summary>
    /// Base asset name
    /// </summary>
    [JsonProperty("an")]
    public string BaseAssetName { get; set; }
    /// <summary>
    /// Quote asset name
    /// </summary>
    [JsonProperty("qn")]
    public string QuoteAssetName { get; set; }

    /// <summary>
    /// Open price
    /// </summary>
    [JsonProperty("o")]
    public decimal? OpenPrice { get; set; }
    /// <summary>
    /// High price
    /// </summary>
    [JsonProperty("h")]
    public decimal? HighPrice { get; set; }
    /// <summary>
    /// Low price
    /// </summary>
    [JsonProperty("l")]
    public decimal? LowPrice { get; set; }
    /// <summary>
    /// Close price
    /// </summary>
    [JsonProperty("c")]
    public decimal? ClosePrice { get; set; }
    /// <summary>
    /// Base volume
    /// </summary>
    [JsonProperty("v")]
    public decimal Volume { get; set; }
    /// <summary>
    /// Quote volume
    /// </summary>
    [JsonProperty("qv")]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// Amount of coins in circulation
    /// </summary>
    [JsonProperty("cs")]
    public decimal? CirculatingSupply { get; set; }

    /// <summary>
    /// Amount of coins in circulation
    /// </summary>
    [JsonProperty("i")]
    public decimal? I { get; set; }
    /// <summary>
    /// Amount of coins in circulation
    /// </summary>
    [JsonProperty("ts")]
    public decimal? Ts { get; set; }
    /// <summary>
    /// Amount of coins in circulation
    /// </summary>
    [JsonProperty("y")]
    public decimal? Y { get; set; }
    /// <summary>
    /// Amount of coins in circulation
    /// </summary>
    [JsonProperty("as")]
    public decimal? As { get; set; }
}
