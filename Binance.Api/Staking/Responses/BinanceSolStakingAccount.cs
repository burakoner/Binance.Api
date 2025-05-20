namespace Binance.Api.Staking;

/// <summary>
/// SOL staking account
/// </summary>
public record BinanceSolStakingAccount
{
    /// <summary>
    /// BN SOL amount
    /// </summary>
    [JsonProperty("bnsolAmount")]
    public decimal BnSolQuantity { get; set; }

    /// <summary>
    /// Holding in SOL
    /// </summary>
    [JsonProperty("holdingInSOL")]
    public decimal HoldingInSOL { get; set; }

    /// <summary>
    /// Thirty days profit in SOL
    /// </summary>
    public decimal ThirtyDaysProfitInSOL { get; set; }
}
