namespace Binance.Api.Futures;

/// <summary>
/// Basis info
/// </summary>
public record BinanceFuturesBasis
{
    /// <summary>
    /// Index price
    /// </summary>
    [JsonProperty("indexPrice")]
    public decimal IndexPrice { get; set; }

    /// <summary>
    /// Contract type
    /// </summary>
    [JsonProperty("contractType")]
    public BinanceFuturesContractType ContractType { get; set; }

    /// <summary>
    /// Basis rate
    /// </summary>
    [JsonProperty("basisRate")]
    public decimal BasisRate { get; set; }

    /// <summary>
    /// Futures price
    /// </summary>
    [JsonProperty("futuresPrice")]
    public decimal FuturesPrice { get; set; }

    /// <summary>
    /// Annualized basis rate
    /// </summary>
    [JsonProperty("annualizedBasisRate")]
    public decimal? AnnualizedBasisRate { get; set; }

    /// <summary>
    /// Basis
    /// </summary>
    [JsonProperty("basis")]
    public decimal Basis { get; set; }

    /// <summary>
    /// The pair
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// Data timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }
}
