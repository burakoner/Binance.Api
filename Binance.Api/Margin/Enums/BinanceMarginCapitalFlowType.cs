namespace Binance.Api.Margin;

/// <summary>Margin capital-flow record type.</summary>
public enum BinanceMarginCapitalFlowType : byte
{
    /// <summary>Transfer.</summary>
    [Map("TRANSFER")]
    Transfer = 1,

    /// <summary>Borrow.</summary>
    [Map("BORROW")]
    Borrow,

    /// <summary>Repayment.</summary>
    [Map("REPAY")]
    Repay,

    /// <summary>Asset received from a buy.</summary>
    [Map("BUY_INCOME")]
    BuyIncome,

    /// <summary>Asset spent on a buy.</summary>
    [Map("BUY_EXPENSE")]
    BuyExpense,

    /// <summary>Asset received from a sell.</summary>
    [Map("SELL_INCOME")]
    SellIncome,

    /// <summary>Asset spent on a sell.</summary>
    [Map("SELL_EXPENSE")]
    SellExpense,

    /// <summary>Trading commission.</summary>
    [Map("TRADING_COMMISSION")]
    TradingCommission,

    /// <summary>Buy-side liquidation.</summary>
    [Map("BUY_LIQUIDATION")]
    BuyLiquidation,

    /// <summary>Sell-side liquidation.</summary>
    [Map("SELL_LIQUIDATION")]
    SellLiquidation,

    /// <summary>Liquidation repayment.</summary>
    [Map("REPAY_LIQUIDATION")]
    RepayLiquidation,

    /// <summary>Other liquidation flow.</summary>
    [Map("OTHER_LIQUIDATION")]
    OtherLiquidation,

    /// <summary>Liquidation fee.</summary>
    [Map("LIQUIDATION_FEE")]
    LiquidationFee,

    /// <summary>Small-balance conversion.</summary>
    [Map("SMALL_BALANCE_CONVERT")]
    SmallBalanceConvert,

    /// <summary>Commission return.</summary>
    [Map("COMMISSION_RETURN")]
    CommissionReturn,

    /// <summary>Small conversion.</summary>
    [Map("SMALL_CONVERT")]
    SmallConvert
}
