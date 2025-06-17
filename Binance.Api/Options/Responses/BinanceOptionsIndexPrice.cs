namespace Binance.Api.Options;

/// <summary>
/// Options Index Price
/// </summary>
public record BinanceOptionsIndexPrice
{
    /// <summary>
    /// Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    /// <summary>
    /// Index Price
    /// </summary>
    public decimal IndexPrice { get; set; }
}