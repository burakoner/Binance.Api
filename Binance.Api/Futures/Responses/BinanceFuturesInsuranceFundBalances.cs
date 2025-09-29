namespace Binance.Api.Futures;

/// <summary>
/// Binance Futures Insurance Fund Balances
/// </summary>
public record BinanceFuturesInsuranceFundBalances
{
    /// <summary>
    /// Symbols
    /// </summary>
    [JsonProperty("symbols")]
    public List<string> Symbols { get; set; } = [];

    /// <summary>
    /// Assets
    /// </summary>
    [JsonProperty("assets")]
    public List<BinanceFuturesInsuranceFundBalance> Assets { get; set; } = [];
}

/// <summary>
/// Binance Futures Insurance Fund Balance
/// </summary>
public record BinanceFuturesInsuranceFundBalance
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Margin Balance
    /// </summary>
    [JsonProperty("marginBalance")]
    public decimal MarginBalance { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
}
