namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Account Configuration
/// </summary>
public record BinancePortfolioMarginAccountConfiguration
{
    /// <summary>
    /// Fee Tier
    /// </summary>
    [JsonProperty("feeTier")]
    public int FeeTier { get; set; }

    /// <summary>
    /// Can Trade
    /// </summary>
    [JsonProperty("canTrade")]
    public bool CanTrade { get; set; }

    /// <summary>
    /// Can Deposit
    /// </summary>
    [JsonProperty("canDeposit")]
    public bool CanDeposit { get; set; }

    /// <summary>
    /// Can Withdraw
    /// </summary>
    [JsonProperty("canWithdraw")]
    public bool CanWithdraw { get; set; }

    /// <summary>
    /// Dual Side Position
    /// </summary>
    [JsonProperty("dualSidePosition")]
    public bool DualSidePosition { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Multi Assets Margin
    /// </summary>
    [JsonProperty("multiAssetsMargin")]
    public bool MultiAssetsMargin { get; set; }

    /// <summary>
    /// Trade Group Id
    /// </summary>
    [JsonProperty("tradeGroupId")]
    public int? TradeGroupId { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Account Configuration for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginAccountConfigurationCM : BinancePortfolioMarginAccountConfiguration
{
}

/// <summary>
/// Binance Portfolio Margin Account Configuration for USDT-Margined Futures
/// </summary>
public record BinancePortfolioMarginAccountConfigurationUM : BinancePortfolioMarginAccountConfiguration
{
}