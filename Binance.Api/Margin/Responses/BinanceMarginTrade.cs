﻿namespace Binance.Api.Margin;

/// <summary>
/// Information about a trade
/// </summary>
public record BinanceMarginTrade
{
    /// <summary>
    /// The symbol the trade is for
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The id of the trade
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The order id the trade belongs to
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Id of the order list this order belongs to
    /// </summary>
    public long? OrderListId { get; set; }

    /// <summary>
    /// The price of the trade
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The quantity of the trade
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The quote quantity of the trade
    /// </summary>
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>
    /// The fee paid for the trade
    /// </summary>
    [JsonProperty("commission")]
    public decimal Fee { get; set; }

    /// <summary>
    /// The asset the fee is paid in
    /// </summary>
    [JsonProperty("commissionAsset")]
    public string FeeAsset { get; set; } = "";

    /// <summary>
    /// The time the trade was made
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Whether account was the buyer in the trade
    /// </summary>
    public bool IsBuyer { get; set; }

    /// <summary>
    /// Whether account was the maker in the trade
    /// </summary>
    public bool IsMaker { get; set; }

    /// <summary>
    /// Whether trade was made with the best match
    /// </summary>
    public bool IsBestMatch { get; set; }

    /// <summary>
    /// If isolated margin (for margin account orders)
    /// </summary>
    public bool? IsIsolated { get; set; }
}
