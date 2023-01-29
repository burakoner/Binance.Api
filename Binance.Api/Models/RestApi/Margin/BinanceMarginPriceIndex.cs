namespace Binance.Api.Models.RestApi.Margin;

/// <summary>
/// Price index for a symbol
/// </summary>
public class BinanceMarginPriceIndex
{
    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// Price
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Time of calculation
    /// </summary>
    [JsonProperty("calcTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CalculationTime { get; set; }
}
