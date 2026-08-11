namespace Binance.Api.Options;

/// <summary>
/// Binance Options Web Socket Stream Mark Price
/// </summary>
public record BinanceOptionsStreamMarkPrice : BinanceSocketStreamEvent
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Mark Price
    /// </summary>
    [JsonProperty("mp")]
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Index price
    /// </summary>
    [JsonProperty("i")]
    public decimal IndexPrice { get; set; }

    /// <summary>
    /// Estimated settlement price
    /// </summary>
    [JsonProperty("P")]
    public decimal EstimatedSettlePrice { get; set; }

    /// <summary>
    /// Best bid price
    /// </summary>
    [JsonProperty("bo")]
    public decimal BestBidPrice { get; set; }

    /// <summary>
    /// Best ask price
    /// </summary>
    [JsonProperty("ao")]
    public decimal BestAskPrice { get; set; }

    /// <summary>
    /// Best bid quantity
    /// </summary>
    [JsonProperty("bq")]
    public decimal BestBidQuantity { get; set; }

    /// <summary>
    /// Best ask quantity
    /// </summary>
    [JsonProperty("aq")]
    public decimal BestAskQuantity { get; set; }

    /// <summary>
    /// Bid implied volatility
    /// </summary>
    [JsonProperty("b")]
    public decimal BidIV { get; set; }

    /// <summary>
    /// Ask implied volatility
    /// </summary>
    [JsonProperty("a")]
    public decimal AskIV { get; set; }

    /// <summary>
    /// High price limit
    /// </summary>
    [JsonProperty("hl")]
    public decimal HighPriceLimit { get; set; }

    /// <summary>
    /// Low price limit
    /// </summary>
    [JsonProperty("ll")]
    public decimal LowPriceLimit { get; set; }

    /// <summary>
    /// Mark implied volatility
    /// </summary>
    [JsonProperty("vo")]
    public decimal MarkIV { get; set; }

    /// <summary>
    /// Risk-free interest rate
    /// </summary>
    [JsonProperty("rf")]
    public decimal RiskFreeInterest { get; set; }

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
}
