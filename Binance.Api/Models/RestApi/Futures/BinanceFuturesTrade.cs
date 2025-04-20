﻿namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// Recent trade info
/// </summary>
public abstract record BinanceRecentTrade : IBinanceRecentTrade
{
    /// <summary>
    /// The id of the trade
    /// </summary>
    [JsonProperty("id")]
    public long OrderId { get; set; }
    /// <summary>
    /// The price of the trade
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }
    /// <inheritdoc />
    public abstract decimal BaseQuantity { get; set; }
    /// <inheritdoc />
    public abstract decimal QuoteQuantity { get; set; }
    /// <summary>
    /// The timestamp of the trade
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TradeTime { get; set; }
    /// <summary>
    /// Whether the buyer is maker
    /// </summary>
    [JsonProperty("isBuyerMaker")]
    public bool BuyerIsMaker { get; set; }
    /// <summary>
    /// Whether the trade was made at the best match
    /// </summary>
    [JsonProperty("isBestMatch")]
    public bool IsBestMatch { get; set; }
}

/// <summary>
/// Recent trade with quote quantity
/// </summary>
public record BinanceRecentTradeQuote : BinanceRecentTrade
{
    /// <inheritdoc />
    [JsonProperty("quoteQty")]
    public override decimal QuoteQuantity { get; set; }

    /// <inheritdoc />
    [JsonProperty("qty")]
    public override decimal BaseQuantity { get; set; }
}

/// <summary>
/// Recent trade with base quantity
/// </summary>
public record BinanceRecentTradeBase : BinanceRecentTrade
{
    /// <inheritdoc />
    [JsonProperty("qty")]
    public override decimal QuoteQuantity { get; set; }

    /// <inheritdoc />
    [JsonProperty("baseQty")]
    public override decimal BaseQuantity { get; set; }
}