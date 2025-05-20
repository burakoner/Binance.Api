namespace Binance.Api.Staking;

/// <summary>
/// Staking Subscribe Result
/// </summary>
public record BinanceSolStakingStake
{
    /// <summary>
    /// Successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// BNSOL Quantity
    /// </summary>
    [JsonProperty("bnsolAmount")]
    public decimal BnSolQuantity { get; set; }

    /// <summary>
    /// Exchange Rate
    /// </summary>
    [JsonProperty("exchangeRate")]
    public decimal ExchangeRate { get; set; }
}
