namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// Price statistics of the last 24 hours
/// </summary>
public record BinanceFuturesCoin24HPrice : Binance24HPriceBase
{
    /// <summary>
    /// The pair the price is for
    /// </summary>
    public string Pair { get; set; } = "";

    [JsonProperty("baseVolume")]
    public override decimal Volume { get; set; }

    [JsonProperty("volume")]
    public override decimal QuoteVolume { get; set; }
}
