namespace Binance.Api.InstitutionalLoan;

/// <summary>
/// Binance Institutional Loan Group
/// </summary>
public record BinanceInstitutionalLoanGroup
{
    /// <summary>
    /// Group Id
    /// </summary>
    public long GroupId { get; set; }

    /// <summary>
    /// Parent Email
    /// </summary>
    public string ParentEmail { get; set; } = "";

    /// <summary>
    /// Credit Email
    /// </summary>
    public string CreditEmail { get; set; } = "";

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// LTV
    /// </summary>
    public decimal LTV { get; set; }

    /// <summary>
    /// Total Net Equity
    /// </summary>
    public decimal TotalNetEquity { get; set; }

    /// <summary>
    /// Total Maintenance Margin
    /// </summary>
    public decimal TotalMaintenanceMargin { get; set; }

    /// <summary>
    /// Total Liability
    /// </summary>
    public decimal TotalLiability { get; set; }

    /// <summary>
    /// Liabilities
    /// </summary>
    public List<BinanceInstitutionalLoanGroupLiability> Liabilities { get; set; } = [];

    /// <summary>
    /// Account Infos
    /// </summary>
    public List<BinanceInstitutionalLoanGroupAccountInfo> AccountInfos { get; set; } = [];
}

/// <summary>
/// Binance Institutional Loan Group Liability
/// </summary>
public record BinanceInstitutionalLoanGroupLiability
{
    /// <summary>
    /// Asset Name
    /// </summary>
    [JsonProperty("assetName")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Principal
    /// </summary>
    public decimal Principal { get; set; }

    /// <summary>
    /// Interest
    /// </summary>
    public decimal Interest { get; set; }
}

/// <summary>
/// Binance Institutional Loan Group Account Info
/// </summary>
public record BinanceInstitutionalLoanGroupAccountInfo
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// Account Type
    /// </summary>
    public string AccountType { get; set; } = "";

    /// <summary>
    /// Net Equity
    /// </summary>
    public decimal NetEquity { get; set; }

    /// <summary>
    /// Maintain Margin
    /// </summary>
    public decimal MaintainMargin { get; set; }
}
