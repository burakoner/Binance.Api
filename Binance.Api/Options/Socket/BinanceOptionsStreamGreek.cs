namespace Binance.Api.Options;

/// <summary>
/// Options Greek update
/// </summary>
public record BinanceOptionsStreamGreek : BinanceSocketStreamEvent
{
    /// <summary>
    /// The listen key the update was received for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TransactionTime { get; set; }

    /// <summary>
    /// Greek values by underlying
    /// </summary>
    [JsonProperty("G")]
    public List<BinanceOptionsStreamGreekValue> Greeks { get; set; } = [];
}

/// <summary>
/// Options Greek values for an underlying
/// </summary>
public record BinanceOptionsStreamGreekValue
{
    /// <summary>
    /// Underlying
    /// </summary>
    [JsonProperty("u")]
    public string Underlying { get; set; } = string.Empty;

    /// <summary>
    /// Delta
    /// </summary>
    [JsonProperty("d")]
    public decimal? Delta { get; set; }

    /// <summary>
    /// Gamma
    /// </summary>
    [JsonProperty("g")]
    public decimal? Gamma { get; set; }

    /// <summary>
    /// Theta
    /// </summary>
    [JsonProperty("t")]
    public decimal? Theta { get; set; }

    /// <summary>
    /// Vega
    /// </summary>
    [JsonProperty("v")]
    public decimal? Vega { get; set; }
}
