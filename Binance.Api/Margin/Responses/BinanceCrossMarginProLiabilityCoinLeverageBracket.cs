namespace Binance.Api.Margin;

/// <summary>
/// Cross margin pro liability coin leverage bracket data
/// </summary>
public record BinanceCrossMarginProLiabilityCoinLeverageBracket
{
    /// <summary>
    /// Asset names
    /// </summary>
    public IEnumerable<string> AssetNames { get; set; } = [];

    /// <summary>
    /// Rank
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    /// Brackets
    /// </summary>
    public IEnumerable<BinanceCrossMarginProBracket> Brackets { get; set; } = [];
}

/// <summary>
/// Cross margin pro bracket data
/// </summary>
public record BinanceCrossMarginProBracket
{
    /// <summary>
    /// Leverage
    /// </summary>
    public int Leverage { get; set; }

    /// <summary>
    /// Max debt
    /// </summary>
    public decimal MaxDebt { get; set; }

    /// <summary>
    /// Maintenance margin rate
    /// </summary>
    public decimal MaintenanceMarginRate { get; set; }

    /// <summary>
    /// Initial margin rate
    /// </summary>
    public decimal InitialMarginRate { get; set; }

    /// <summary>
    /// Fast num
    /// </summary>
    public decimal FastNum { get; set; }
}