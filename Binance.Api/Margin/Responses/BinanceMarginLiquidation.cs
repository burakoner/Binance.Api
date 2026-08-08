namespace Binance.Api.Margin;

/// <summary>Result of manually liquidating a Margin account.</summary>
public record BinanceMarginManualLiquidation
{
    /// <summary>Liquidated asset.</summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>Interest repaid during liquidation.</summary>
    public decimal Interest { get; set; }

    /// <summary>Principal repaid during liquidation.</summary>
    public decimal Principal { get; set; }

    /// <summary>Asset in which the remaining liability is denominated.</summary>
    public string LiabilityAsset { get; set; } = string.Empty;

    /// <summary>Remaining liability quantity.</summary>
    [JsonProperty("liabilityQty")]
    public decimal LiabilityQuantity { get; set; }
}

/// <summary>Current Cross Margin liquidation-loan balance.</summary>
public record BinanceMarginLiquidationLoan
{
    /// <summary>Liquidation-loan asset.</summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>Total liquidation-loan amount.</summary>
    [JsonProperty("amount")]
    public decimal TotalAmount { get; set; }

    /// <summary>Amount already repaid.</summary>
    public decimal RepaidAmount { get; set; }

    /// <summary>Amount still outstanding.</summary>
    public decimal RemainingAmount { get; set; }
}

/// <summary>One Cross Margin liquidation-loan repayment.</summary>
public record BinanceMarginLiquidationLoanRepayment
{
    /// <summary>Repayment transaction identifier.</summary>
    [JsonProperty("repayId")]
    public long RepaymentId { get; set; }

    /// <summary>Asset used for repayment.</summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>Actual repayment amount.</summary>
    public decimal Amount { get; set; }

    /// <summary>Repayment state.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceMarginLiquidationLoanRepaymentStatus Status { get; set; }

    /// <summary>Time the repayment was created.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }
}

/// <summary>Paginated Cross Margin liquidation-loan repayment history.</summary>
public record BinanceMarginLiquidationLoanRepaymentHistory
{
    /// <summary>Total number of repayment records.</summary>
    public long Total { get; set; }

    /// <summary>Repayment records.</summary>
    public List<BinanceMarginLiquidationLoanRepayment> Rows { get; set; } = [];
}
