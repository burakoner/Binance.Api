using Binance.Net.Enums;

namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Account info
    /// </summary>
    public record BinanceFuturesAccountInfoV3
    {
        /// <summary>
        /// Total initial margin
        /// </summary>
        [JsonProperty("totalInitialMargin")]
        public decimal TotalInitialMargin { get; set; }
        /// <summary>
        /// Total maintenance margin
        /// </summary>
        [JsonProperty("totalMaintMargin")]
        public decimal TotalMaintenanceMargin { get; set; }
        /// <summary>
        /// Total wallet balance
        /// </summary>
        [JsonProperty("totalWalletBalance")]
        public decimal TotalWalletBalance { get; set; }
        /// <summary>
        /// Total unrealized profit
        /// </summary>
        [JsonProperty("totalUnrealizedProfit")]
        public decimal TotalUnrealizedProfit { get; set; }
        /// <summary>
        /// Total margin balance
        /// </summary>
        [JsonProperty("totalMarginBalance")]
        public decimal TotalMarginBalance { get; set; }
        /// <summary>
        /// Total position initial margin
        /// </summary>
        [JsonProperty("totalPositionInitialMargin")]
        public decimal TotalPositionInitialMargin { get; set; }
        /// <summary>
        /// Total open order initial margin
        /// </summary>
        [JsonProperty("totalOpenOrderInitialMargin")]
        public decimal TotalOpenOrderInitialMargin { get; set; }
        /// <summary>
        /// Total cross wallet balance
        /// </summary>
        [JsonProperty("totalCrossWalletBalance")]
        public decimal TotalCrossWalletBalance { get; set; }
        /// <summary>
        /// Total cross unrealized profit and loss
        /// </summary>
        [JsonProperty("totalCrossUnPnl")]
        public decimal TotalCrossUnrealizedPnl { get; set; }
        /// <summary>
        /// Available balance
        /// </summary>
        [JsonProperty("availableBalance")]
        public decimal AvailableBalance { get; set; }
        /// <summary>
        /// Max withdraw quantity
        /// </summary>
        [JsonProperty("maxWithdrawAmount")]
        public decimal MaxWithdrawQuantity { get; set; }
        /// <summary>
        /// Assets
        /// </summary>
        [JsonProperty("assets")]
        public IEnumerable<BinanceFuturesAccountInfoAsset> Assets { get; set; } = Array.Empty<BinanceFuturesAccountInfoAsset>();
        /// <summary>
        /// Positions
        /// </summary>
        [JsonProperty("positions")]
        public IEnumerable<BinanceFuturesAccountInfoPosition> Positions { get; set; } = Array.Empty<BinanceFuturesAccountInfoPosition>();
    }

    /// <summary>
    /// Asset information
    /// </summary>
    public record BinanceFuturesAccountInfoAsset
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonProperty("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Wallet balance
        /// </summary>
        [JsonProperty("walletBalance")]
        public decimal WalletBalance { get; set; }
        /// <summary>
        /// Unrealized profit
        /// </summary>
        [JsonProperty("unrealizedProfit")]
        public decimal UnrealizedProfit { get; set; }
        /// <summary>
        /// Margin balance
        /// </summary>
        [JsonProperty("marginBalance")]
        public decimal MarginBalance { get; set; }
        /// <summary>
        /// Maintenance margin
        /// </summary>
        [JsonProperty("maintMargin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// Initial margin
        /// </summary>
        [JsonProperty("initialMargin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// Position initial margin
        /// </summary>
        [JsonProperty("positionInitialMargin")]
        public decimal PositionInitialMargin { get; set; }
        /// <summary>
        /// Open order initial margin
        /// </summary>
        [JsonProperty("openOrderInitialMargin")]
        public decimal OpenOrderInitialMargin { get; set; }
        /// <summary>
        /// Cross wallet balance
        /// </summary>
        [JsonProperty("crossWalletBalance")]
        public decimal CrossWalletBalance { get; set; }
        /// <summary>
        /// Cross unrealized profit and loss
        /// </summary>
        [JsonProperty("crossUnPnl")]
        public decimal CrossUnrealizedPnl { get; set; }
        /// <summary>
        /// Available balance
        /// </summary>
        [JsonProperty("availableBalance")]
        public decimal AvailableBalance { get; set; }
        /// <summary>
        /// Max withdraw quantity
        /// </summary>
        [JsonProperty("maxWithdrawAmount")]
        public decimal MaxWithdrawQuantity { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonProperty("updateTime")]
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// Position info
    /// </summary>
    public record BinanceFuturesAccountInfoPosition
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Position side
        /// </summary>
        [JsonProperty("positionSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Position amount
        /// </summary>
        [JsonProperty("positionAmt")]
        public decimal PositionAmount { get; set; }
        /// <summary>
        /// Unrealized profit
        /// </summary>
        [JsonProperty("unrealizedProfit")]
        public decimal UnrealizedProfit { get; set; }
        /// <summary>
        /// Isolated margin
        /// </summary>
        [JsonProperty("isolatedMargin")]
        public decimal IsolatedMargin { get; set; }
        /// <summary>
        /// Notional
        /// </summary>
        [JsonProperty("notional")]
        public decimal Notional { get; set; }
        /// <summary>
        /// Isolated wallet
        /// </summary>
        [JsonProperty("isolatedWallet")]
        public decimal IsolatedWallet { get; set; }
        /// <summary>
        /// Initial margin
        /// </summary>
        [JsonProperty("initialMargin")]
        public decimal InitialMargin { get; set; }
        /// <summary>
        /// Maintenance margin
        /// </summary>
        [JsonProperty("maintMargin")]
        public decimal MaintenanceMargin { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonProperty("updateTime")]
        public DateTime? UpdateTime { get; set; }
    }


}
