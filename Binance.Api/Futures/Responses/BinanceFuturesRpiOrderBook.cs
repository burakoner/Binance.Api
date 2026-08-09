namespace Binance.Api.Futures;

/// <summary>
/// Futures order book including aggregated RPI orders
/// </summary>
public record BinanceFuturesRpiOrderBook
{
    /// <summary>
    /// Last update identifier
    /// </summary>
    public long LastUpdateId { get; set; }

    /// <summary>
    /// Raw message output time published as E by Binance
    /// </summary>
    [JsonProperty("E")]
    public long MessageOutputTime { get; set; }

    /// <summary>
    /// Raw transaction time published as T by Binance
    /// </summary>
    [JsonProperty("T")]
    public long TransactionTime { get; set; }

    /// <summary>
    /// Bid price levels
    /// </summary>
    public List<BinanceFuturesOrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// Ask price levels
    /// </summary>
    public List<BinanceFuturesOrderBookEntry> Asks { get; set; } = [];
}
