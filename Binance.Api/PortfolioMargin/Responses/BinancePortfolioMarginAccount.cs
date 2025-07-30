namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Account
/// </summary>
public record BinancePortfolioMarginAccount
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Total Wallet Balance
    /// </summary>
    [JsonProperty("totalWalletBalance")]
    public decimal TotalWalletBalance { get; set; }

    /// <summary>
    /// Cross Wallet Balance
    /// </summary>
    [JsonProperty("crossMarginBorrowed")]
    public decimal CrossMarginBorrowed { get; set; }

    /// <summary>
    /// Cross Margin Free
    /// </summary>
    [JsonProperty("crossMarginFree")]
    public decimal CrossMarginFree { get; set; }

    /// <summary>
    /// Cross Margin Interest
    /// </summary>
    [JsonProperty("crossMarginInterest")]
    public decimal CrossMarginInterest { get; set; }

    /// <summary>
    /// Cross Margin Locked
    /// </summary>
    [JsonProperty("crossMarginLocked")]
    public decimal CrossMarginLocked { get; set; }

    /// <summary>
    /// UM Wallet Balance
    /// </summary>
    [JsonProperty("umWalletBalance")]
    public decimal UMWalletBalance { get; set; }

    /// <summary>
    /// UM Unrealized Profit and Loss
    /// </summary>
    [JsonProperty("umUnrealizedPNL")]
    public decimal UMUnrealizedPNL { get; set; }

    /// <summary>
    /// CM Wallet Balance
    /// </summary>
    [JsonProperty("cmWalletBalance")]
    public decimal CMWalletBalance { get; set; }

    /// <summary>
    /// CM Unrealized Profit and Loss
    /// </summary>
    [JsonProperty("cmUnrealizedPNL")]
    public decimal CMUnrealizedPNL { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Negative Balance
    /// </summary>
    [JsonProperty("negativeBalance")]
    public decimal NegativeBalance { get; set; }
}

#region V1

/// <summary>
/// Binance Portfolio Margin Account for USD-Margined Futures
/// </summary>
public record BinancePortfolioMarginAccountUM
{
    /// <summary>
    /// Assets
    /// </summary>
    [JsonProperty("assets")]
    public List<BinancePortfolioMarginAccountAssetUM> Assets { get; set; } = [];

    /// <summary>
    /// Positions
    /// </summary>
    [JsonProperty("positions")]
    public List<BinancePortfolioMarginAccountPositionUM> Positions { get; set; } = [];
}

/// <summary>
/// Binance Portfolio Margin Account for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginAccountCM
{
    /// <summary>
    /// Assets
    /// </summary>
    [JsonProperty("assets")]
    public List<BinancePortfolioMarginAccountAssetCM> Assets { get; set; } = [];

    /// <summary>
    /// Positions
    /// </summary>
    [JsonProperty("positions")]
    public List<BinancePortfolioMarginAccountPositionCM> Positions { get; set; } = [];
}

/// <summary>
/// Binance Portfolio Margin Account Asset
/// </summary>
public record BinancePortfolioMarginAccountAsset
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Cross Wallet Balance
    /// </summary>
    [JsonProperty("crossWalletBalance")]
    public decimal CrossWalletBalance { get; set; }

    /// <summary>
    /// Cross Unrealized Profit and Loss
    /// </summary>
    [JsonProperty("crossUnPnl")]
    public decimal CrossUnrealizedUnPnl { get; set; }

    /// <summary>
    /// Maintenance Margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    /// <summary>
    /// Initial Margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Position Initial Margin
    /// </summary>
    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }

    /// <summary>
    /// Open Order Initial Margin
    /// </summary>
    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Account Asset for USD-Margined Futures
/// </summary>
public record BinancePortfolioMarginAccountAssetCM : BinancePortfolioMarginAccountAsset
{
}

/// <summary>
/// Binance Portfolio Margin Account Asset for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginAccountAssetUM: BinancePortfolioMarginAccountAsset
{
}

/// <summary>
/// Binance Portfolio Margin Account Position
/// </summary>
public record BinancePortfolioMarginAccountPosition
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Initial Margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Maintenance Margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    /// <summary>
    /// Unrealized Profit
    /// </summary>
    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedProfit { get; set; }

    /// <summary>
    /// Position Initial Margin
    /// </summary>
    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }

    /// <summary>
    /// Open Order Initial Margin
    /// </summary>
    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }

    /// <summary>
    /// Leverage
    /// </summary>
    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }

    /// <summary>
    /// Entry Price
    /// </summary>
    [JsonProperty("entryPrice")]
    public decimal EntryPrice { get; set; }

    /// <summary>
    /// Position Side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    /// <summary>
    /// Position Quantity
    /// </summary>
    [JsonProperty("positionAmt")]
    public decimal PositionQuantity { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Account Position for USD-Margined Futures
/// </summary>
public record BinancePortfolioMarginAccountPositionCM : BinancePortfolioMarginAccountPosition
{
    /// <summary>
    /// Maximum Quantity
    /// </summary>
    [JsonProperty("maxQty")]
    public decimal MaximumQuantity { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Account Position for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginAccountPositionUM: BinancePortfolioMarginAccountPosition
{
    /// <summary>
    /// Maximum Notional
    /// </summary>
    [JsonProperty("maxNotional")]
    public decimal MaximumNotional { get; set; }

    /// <summary>
    /// Bid Notional
    /// </summary>
    [JsonProperty("bidNotional")]
    public decimal BidNotional { get; set; }

    /// <summary>
    /// Ask Notional
    /// </summary>
    [JsonProperty("askNotional")]
    public decimal AskNotional { get; set; }

}

#endregion

#region V2
/// <summary>
/// Binance Portfolio Margin Account for USD-Margined Futures (V2)
/// </summary>
public record BinancePortfolioMarginAccountUMV2
{
    /// <summary>
    /// Assets
    /// </summary>
    [JsonProperty("assets")]
    public List<BinancePortfolioMarginAccountAssetUMV2> Assets { get; set; } = [];

    /// <summary>
    /// Positions
    /// </summary>
    [JsonProperty("positions")]
    public List<BinancePortfolioMarginAccountPositionUMV2> Positions { get; set; } = [];
}

/// <summary>
/// Binance Portfolio Margin Account for Coin-Margined Futures (V2)
/// </summary>
public record BinancePortfolioMarginAccountCMV2
{
    /// <summary>
    /// Assets
    /// </summary>
    [JsonProperty("assets")]
    public List<BinancePortfolioMarginAccountAssetCMV2> Assets { get; set; } = [];

    /// <summary>
    /// Positions
    /// </summary>
    [JsonProperty("positions")]
    public List<BinancePortfolioMarginAccountPositionCMV2> Positions { get; set; } = [];
}

/// <summary>
/// Binance Portfolio Margin Account Asset (V2)
/// </summary>
public record BinancePortfolioMarginAccountAssetV2
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Cross Wallet Balance
    /// </summary>
    [JsonProperty("crossWalletBalance")]
    public decimal CrossWalletBalance { get; set; }

    /// <summary>
    /// Cross Unrealized Profit and Loss
    /// </summary>
    [JsonProperty("crossUnPnl")]
    public decimal CrossUnrealizedUnPnl { get; set; }

    /// <summary>
    /// Maintenance Margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    /// <summary>
    /// Initial Margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Position Initial Margin
    /// </summary>
    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }

    /// <summary>
    /// Open Order Initial Margin
    /// </summary>
    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Account Asset for USD-Margined Futures (V2)
/// </summary>
public record BinancePortfolioMarginAccountAssetCMV2 : BinancePortfolioMarginAccountAssetV2
{
}

/// <summary>
/// Binance Portfolio Margin Account Asset for Coin-Margined Futures (V2)
/// </summary>
public record BinancePortfolioMarginAccountAssetUMV2 : BinancePortfolioMarginAccountAssetV2
{
}

/// <summary>
/// Binance Portfolio Margin Account Position (V2)
/// </summary>
public record BinancePortfolioMarginAccountPositionV2
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Initial Margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Maintenance Margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    /// <summary>
    /// Unrealized Profit
    /// </summary>
    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedProfit { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Account Position for USD-Margined Futures (V2)
/// </summary>
public record BinancePortfolioMarginAccountPositionCMV2 : BinancePortfolioMarginAccountPositionV2
{
}

/// <summary>
/// Binance Portfolio Margin Account Position for Coin-Margined Futures (V2)
/// </summary>
public record BinancePortfolioMarginAccountPositionUMV2 : BinancePortfolioMarginAccountPositionV2
{
    /// <summary>
    /// Position Side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    /// <summary>
    /// Position Quantity
    /// </summary>
    [JsonProperty("positionAmt")]
    public decimal PositionQuantity { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Notional
    /// </summary>
    [JsonProperty("notional")]
    public decimal Notional { get; set; }
}

#endregion