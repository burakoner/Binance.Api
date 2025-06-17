namespace Binance.Api.Options;

/// <summary>
/// Price statistics of the last 24 hours
/// </summary>
public record BinanceOptionsStreamTicker : BinanceSocketStreamEvent
{
    /// <summary>
    /// Transaction time of the ticker
    /// </summary>
    [JsonProperty("T")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The symbol the price is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The open price 24 hours ago
    /// </summary>
    [JsonProperty("o")]
    public decimal Open { get; set; }

    /// <summary>
    /// The highest price in the last 24 hours
    /// </summary>
    [JsonProperty("h")]
    public decimal High { get; set; }

    /// <summary>
    /// The lowest price in the last 24 hours
    /// </summary>
    [JsonProperty("l")]
    public decimal Low { get; set; }

    /// <summary>
    /// The most recent trade price
    /// </summary>
    [JsonProperty("c")]
    public decimal LastPrice { get; set; }

    /// <summary>
    /// The base volume traded in the last 24 hours
    /// </summary>
    [JsonProperty("V")]
    public decimal Volume { get; set; }

    /// <summary>
    /// The quote asset volume traded in the last 24 hours
    /// </summary>
    [JsonProperty("A")]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// The price change in percentage in the last 24 hours
    /// </summary>
    [JsonProperty("P")]
    public decimal PriceChangePercent { get; set; }

    /// <summary>
    /// The actual price change in the last 24 hours
    /// </summary>
    [JsonProperty("p")]
    public decimal PriceChange { get; set; }

    /// <summary>
    /// The most recent trade quantity
    /// </summary>
    [JsonProperty("Q")]
    public decimal LastQuantity { get; set; }

    /// <summary>
    /// The first trade ID in the last 24 hours
    /// </summary>
    [JsonProperty("F")]
    public long FirstTradeId { get; set; }

    /// <summary>
    /// The last trade ID in the last 24 hours
    /// </summary>
    [JsonProperty("L")]
    public long LastTradeId { get; set; }

    /// <summary>
    /// number of trades
    /// </summary>
    [JsonProperty("n")]
    public long TradeCount { get; set; }

    /// <summary>
    /// The best bid price in the order book
    /// </summary>
    [JsonProperty("bo")]
    public decimal BestBidPrice { get; set; }

    /// <summary>
    /// The best ask price in the order book
    /// </summary>
    [JsonProperty("ao")]
    public decimal BestAskPrice { get; set; }

    /// <summary>
    /// The quantity of the best ask price in the order book
    /// </summary>
    [JsonProperty("aq")]
    public decimal BestAskQuantity { get; set; }

    /// <summary>
    /// The quantity of the best bid price in the order book
    /// </summary>
    [JsonProperty("bq")]
    public decimal BestBidQuantity { get; set; }

    /// <summary>
    /// BuyImplied volatility   
    /// </summary>
    [JsonProperty("b")]
    public decimal BuyImpliedVolatility { get; set; }

    /// <summary>
    /// SellImplied volatility
    /// </summary>
    [JsonProperty("a")]
    public decimal ExercisePrice { get; set; }

    /// <summary>
    /// Delta
    /// </summary>
    [JsonProperty("d")]
    public decimal Delta { get; set; }

    /// <summary>
    /// Theta 
    /// </summary>
    [JsonProperty("t")]
    public decimal Theta { get; set; }

    /// <summary>
    /// Gamma
    /// </summary>
    [JsonProperty("g")]
    public decimal Gamma { get; set; }

    /// <summary>
    /// Vega
    /// </summary>
    [JsonProperty("v")]
    public decimal Vega { get; set; }

    /// <summary>
    /// Implied volatility 
    /// </summary>
    [JsonProperty("vo")]
    public decimal ImpliedVolatility { get; set; }

    /// <summary>
    /// Mark price
    /// </summary>
    [JsonProperty("mp")]
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Buy Maximum price
    /// </summary>
    [JsonProperty("hl")]
    public decimal BuyMaximumPrice { get; set; }

    /// <summary>
    /// Sell Minimum price 
    /// </summary>
    [JsonProperty("ll")]
    public decimal SellMinimumPrice { get; set; }

    /// <summary>
    /// Estimated strike price
    /// </summary>
    [JsonProperty("eep")]
    public decimal EstimatedStrikePrice { get; set; }
}