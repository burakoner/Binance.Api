namespace Binance.Api.Staking;

/// <summary>
/// Staking result
/// </summary>
public record BinanceSolStakingResult
{
    /// <summary>
    /// Successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// BN SOL amount
    /// </summary>
    [JsonProperty("bnsolAmount")]
    public decimal BnSolAmount { get; set; }

    /// <summary>
    /// Exchange rate
    /// </summary>
    [JsonProperty("exchangeRate")]
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// Arrival time for redeeming
    /// </summary>
    [JsonProperty("arrivalTime")]
    public DateTime? ArrivalTime { get; set; }
}
