namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginAccount
{
    [JsonProperty("asset")]
    public string Asset { get; set; }

    [JsonProperty("totalWalletBalance")]
    public decimal TotalWalletBalance { get; set; }

    [JsonProperty("crossMarginBorrowed")]
    public decimal CrossMarginBorrowed { get; set; }

    [JsonProperty("crossMarginFree")]
    public decimal CrossMarginFree { get; set; }

    [JsonProperty("crossMarginInterest")]
    public decimal CrossMarginInterest { get; set; }

    [JsonProperty("crossMarginLocked")]
    public decimal CrossMarginLocked { get; set; }

    [JsonProperty("umWalletBalance")]
    public decimal UMWalletBalance { get; set; }

    [JsonProperty("umUnrealizedPNL")]
    public decimal UMUnrealizedPNL { get; set; }

    [JsonProperty("cmWalletBalance")]
    public decimal CMWalletBalance { get; set; }

    [JsonProperty("cmUnrealizedPNL")]
    public decimal CMUnrealizedPNL { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    [JsonProperty("negativeBalance")]
    public decimal NegativeBalance { get; set; }
}

#region V1

public record BinancePortfolioMarginAccountUM
{
    [JsonProperty("assets")]
    public List<BinancePortfolioMarginAccountAssetUM> Assets { get; set; }

    [JsonProperty("positions")]
    public List<BinancePortfolioMarginAccountPositionUM> Positions { get; set; }
}

public record BinancePortfolioMarginAccountCM
{
    [JsonProperty("assets")]
    public List<BinancePortfolioMarginAccountAssetCM> Assets { get; set; }

    [JsonProperty("positions")]
    public List<BinancePortfolioMarginAccountPositionCM> Positions { get; set; }
}

public record BinancePortfolioMarginAccountAsset
{
    [JsonProperty("asset")]
    public string Asset { get; set; }

    [JsonProperty("crossWalletBalance")]
    public decimal CrossWalletBalance { get; set; }

    [JsonProperty("crossUnPnl")]
    public decimal CrossUnrealizedUnPnl { get; set; }

    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }

    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

public record BinancePortfolioMarginAccountAssetCM : BinancePortfolioMarginAccountAsset
{
}

public record BinancePortfolioMarginAccountAssetUM: BinancePortfolioMarginAccountAsset
{
}

public record BinancePortfolioMarginAccountPosition
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedProfit { get; set; }

    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }

    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }

    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }

    [JsonProperty("entryPrice")]
    public decimal EntryPrice { get; set; }

    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    [JsonProperty("positionAmt")]
    public decimal PositionQuantity { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

public record BinancePortfolioMarginAccountPositionCM : BinancePortfolioMarginAccountPosition
{
    [JsonProperty("maxQty")]
    public decimal MaximumQuantity { get; set; }
}

public record BinancePortfolioMarginAccountPositionUM: BinancePortfolioMarginAccountPosition
{
    [JsonProperty("maxNotional")]
    public decimal MaximumNotional { get; set; }

    [JsonProperty("bidNotional")]
    public decimal BidNotional { get; set; }

    [JsonProperty("askNotional")]
    public decimal AskNotional { get; set; }

}

#endregion

#region V2

public record BinancePortfolioMarginAccountUMV2
{
    [JsonProperty("assets")]
    public List<BinancePortfolioMarginAccountAssetUMV2> Assets { get; set; }

    [JsonProperty("positions")]
    public List<BinancePortfolioMarginAccountPositionUMV2> Positions { get; set; }
}

public record BinancePortfolioMarginAccountCMV2
{
    [JsonProperty("assets")]
    public List<BinancePortfolioMarginAccountAssetCMV2> Assets { get; set; }

    [JsonProperty("positions")]
    public List<BinancePortfolioMarginAccountPositionCMV2> Positions { get; set; }
}

public record BinancePortfolioMarginAccountAssetV2
{
    [JsonProperty("asset")]
    public string Asset { get; set; }

    [JsonProperty("crossWalletBalance")]
    public decimal CrossWalletBalance { get; set; }

    [JsonProperty("crossUnPnl")]
    public decimal CrossUnrealizedUnPnl { get; set; }

    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }

    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

public record BinancePortfolioMarginAccountAssetCMV2 : BinancePortfolioMarginAccountAssetV2
{
}

public record BinancePortfolioMarginAccountAssetUMV2 : BinancePortfolioMarginAccountAssetV2
{
}

public record BinancePortfolioMarginAccountPositionV2
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedProfit { get; set; }
}

public record BinancePortfolioMarginAccountPositionCMV2 : BinancePortfolioMarginAccountPositionV2
{
}

public record BinancePortfolioMarginAccountPositionUMV2 : BinancePortfolioMarginAccountPositionV2
{
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    [JsonProperty("positionAmt")]
    public decimal PositionQuantity { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    [JsonProperty("notional")]
    public decimal Notional { get; set; }
}

#endregion