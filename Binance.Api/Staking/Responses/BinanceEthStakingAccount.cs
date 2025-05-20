namespace Binance.Api.Staking;

/// <summary>
/// Eth staking account
/// </summary>
public record BinanceEthStakingAccount
{
    /// <summary>
    /// Holdings in ETH Staking Account in ETH
    /// </summary>
    [JsonProperty("holdingInETH")]
    public decimal HoldingInETH { get; set; }

    /// <summary>
    /// Holdings in ETH Staking Account
    /// </summary>
    [JsonProperty("holdings")]
    public BinanceEthStakingAccountHoldings Holdings { get; set; } = new();

    /// <summary>
    /// Thirty Days Profit in ETH Staking Account in ETH
    /// </summary>
    [JsonProperty("thirtyDaysProfitInETH")]
    public decimal ThirtyDaysProfitInETH { get; set; }

    /// <summary>
    /// Profit in ETH Staking Account in ETH
    /// </summary>
    [JsonProperty("profit")]
    public BinanceEthStakingAccountProfit Profit { get; set; } = new();

}

/// <summary>
/// Binance ETH Staking Account Holdings
/// </summary>
public record BinanceEthStakingAccountHoldings
{
    /// <summary>
    /// WBETH Quantity
    /// </summary>
    [JsonProperty("wbethAmount")]
    public decimal WBethQuantity { get; set; }

    /// <summary>
    /// Beth Quantity
    /// </summary>
    [JsonProperty("bethAmount")]
    public decimal BethQuantity { get; set; }
}

/// <summary>
/// Binance ETH Staking Account Profit
/// </summary>
public record BinanceEthStakingAccountProfit
{
    /// <summary>
    /// Quantity from WBETH
    /// </summary>
    [JsonProperty("amountFromWBETH")]
    public decimal QuantityFromWBETH { get; set; }

    /// <summary>
    /// Quantity from BETH
    /// </summary>
    [JsonProperty("amountFromBETH")]
    public decimal QuantityFromBETH { get; set; }
}
