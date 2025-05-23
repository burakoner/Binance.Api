﻿namespace Binance.Api.Futures;

/// <summary>
/// The history of order edits
/// </summary>
public record BinanceFuturesOrderEditHistory
{
    /// <summary>
    /// The symbol the order is for
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string? Pair { get; set; }

    /// <summary>
    /// The id of the amendment
    /// </summary>
    [JsonProperty("amendmentId")]
    public long AmendmentId { get; set; }

    /// <summary>
    /// The order id as assigned by Binance
    /// </summary>
    [JsonProperty("orderId")]
    public long Id { get; set; }

    /// <summary>
    /// The order id as assigned by the client
    /// </summary>
    [JsonProperty("clientOrderId")]
    public string? ClientOrderId { get; set; } = string.Empty;

    /// <summary>
    /// The order id as assigned by the client without the prefix
    /// </summary>
    public string? RequestClientOrderId => ClientOrderId?
        .TrimStart(BinanceConstants.ClientOrderIdPrefixSpot.ToCharArray())
        .TrimStart(BinanceConstants.ClientOrderIdPrefixFutures.ToCharArray());

    /// <summary>
    /// Edit time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Edit info
    /// </summary>
    [JsonProperty("amendment")]
    public BinanceFuturesOrderChanges EditInfo { get; set; } = null!;

    /// <summary>
    /// Price match
    /// </summary>
    [JsonProperty("priceMatch")]
    public BinanceFuturesPriceMatch PriceMatch { get; set; }
}

/// <summary>
/// Order changes
/// </summary>
public record BinanceFuturesOrderChanges
{
    /// <summary>
    /// Price change
    /// </summary>
    [JsonProperty("price")]
    public BinanceFuturesOrderChange Price { get; set; } = null!;

    /// <summary>
    /// Quantity change
    /// </summary>
    [JsonProperty("origQty")]
    public BinanceFuturesOrderChange Quantity { get; set; } = null!;


    /// <summary>
    /// Amount of times changed
    /// </summary>
    [JsonProperty("count")]
    public int EditCount { get; set; }
}

/// <summary>
/// Change info
/// </summary>
public record BinanceFuturesOrderChange
{
    /// <summary>
    /// Before edit
    /// </summary>
    [JsonProperty("before")]
    public decimal Before { get; set; }

    /// <summary>
    /// After edit
    /// </summary>
    [JsonProperty("after")]
    public decimal After { get; set; }
}
