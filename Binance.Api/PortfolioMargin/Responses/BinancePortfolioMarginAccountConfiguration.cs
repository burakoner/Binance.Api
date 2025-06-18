namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginAccountConfiguration
{
    [JsonProperty("feeTier")]
    public int FeeTier { get; set; }

    [JsonProperty("canTrade")]
    public bool CanTrade { get; set; }

    [JsonProperty("canDeposit")]
    public bool CanDeposit { get; set; }

    [JsonProperty("canWithdraw")]
    public bool CanWithdraw { get; set; }

    [JsonProperty("dualSidePosition")]
    public bool DualSidePosition { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    [JsonProperty("multiAssetsMargin")]
    public bool MultiAssetsMargin { get; set; }

    [JsonProperty("tradeGroupId")]
    public int? TradeGroupId { get; set; }
}

public record BinancePortfolioMarginAccountConfigurationCM : BinancePortfolioMarginAccountConfiguration
{
}

public record BinancePortfolioMarginAccountConfigurationUM : BinancePortfolioMarginAccountConfiguration
{
}