﻿namespace Binance.Api.Spot;

/// <summary>
/// Aggregated information about trades for a symbol
/// </summary>
public record BinanceSpotStreamAggregatedTrade : BinanceSocketStreamEvent
{
    /// <summary>
    /// The symbol the trade was for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The id of this aggregated trade
    /// </summary>
    [JsonProperty("a")]
    public long Id { get; set; }

    /// <summary>
    /// The price of the trades
    /// </summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>
    /// The combined quantity of the trades
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The first trade id in this aggregation
    /// </summary>
    [JsonProperty("f")]
    public long FirstTradeId { get; set; }

    /// <summary>
    /// The last trade id in this aggregation
    /// </summary>
    [JsonProperty("l")]
    public long LastTradeId { get; set; }

    /// <summary>
    /// The time of the trades
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TradeTime { get; set; }

    /// <summary>
    /// Whether the buyer was the maker
    /// </summary>
    [JsonProperty("m")]
    public bool BuyerIsMaker { get; set; }

    /// <summary>
    /// Unused
    /// </summary>
    [JsonProperty("M")]
    public bool Ignore { get; set; }
}
