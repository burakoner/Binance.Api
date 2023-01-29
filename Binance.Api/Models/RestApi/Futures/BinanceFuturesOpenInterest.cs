namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// Open interest
/// </summary>
public class BinanceFuturesOpenInterest
{
    /// <summary>
    /// The symbol the information is about
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// Open Interest info
    /// </summary>
    public decimal OpenInterest { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? Timestamp { get; set; }
}

/// <summary>
/// Open interest
/// </summary>
public class BinanceFuturesCoinOpenInterest : BinanceFuturesOpenInterest
{
    /// <summary>
    /// The pair
    /// </summary>
    public string Pair { get; set; }
    /// <summary>
    /// The contract type
    /// </summary>
    [JsonConverter(typeof(ContractTypeConverter))]
    public ContractType ContractType { get; set; }
}