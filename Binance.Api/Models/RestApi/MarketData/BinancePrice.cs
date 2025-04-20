﻿namespace Binance.Api.Models.RestApi.MarketData;

/// <summary>
/// The price of a symbol
/// </summary>
public record BinancePrice
{
    /// <summary>
    /// The symbol the price is for
    /// </summary>
    public string Symbol { get; set; } = "";
    /// <summary>
    /// The price of the symbol
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? Timestamp { get; set; }
}

/// <summary>
/// Futures-Coin price
/// </summary>
public record BinanceFuturesCoinPrice : BinancePrice
{
    /// <summary>
    /// Name of the pair
    /// </summary>
    [JsonProperty("ps")]
    public string Pair { get; set; } = "";
}
