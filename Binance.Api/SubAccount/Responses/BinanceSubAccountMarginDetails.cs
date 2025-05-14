using Binance.Api.Margin;

namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account margin trade details
/// </summary>
public record BinanceSubAccountMarginDetails
{
    /// <summary>
    /// Email of the account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Margin level
    /// </summary>
    [JsonProperty("marginLevel")]
    public decimal MarginLevel { get; set; }

    /// <summary>
    /// Total asset in btc
    /// </summary>
    [JsonProperty("totalAssetOfBtc")]
    public decimal TotalAssetOfBtc { get; set; }

    /// <summary>
    /// Total liability
    /// </summary>
    [JsonProperty("totalLiabilityOfBtc")]
    public decimal TotalLiabilityOfBtc { get; set; }

    /// <summary>
    /// Total net asset
    /// </summary>
    [JsonProperty("totalNetAssetOfBtc")]
    public decimal TotalNetAssetOfBtc { get; set; }

    /// <summary>
    /// Trade details
    /// </summary>
    [JsonProperty("marginTradeCoeffVo")]
    public BinanceMarginTradeCoeff? MarginTradeCoeff { get; set; }

    /// <summary>
    /// Asset list
    /// </summary>
    [JsonProperty("marginUserAssetVoList")]
    public BinanceMarginAccountBalance[] MarginUserAssets { get; set; } = [];
}

/// <summary>
/// Margin trade detail
/// </summary>
public record BinanceMarginTradeCoeff
{
    /// <summary>
    /// Liquidation margin ratio
    /// </summary>
    [JsonProperty("forceLiquidationBar")]
    public decimal ForceLiquidationBar { get; set; }

    /// <summary>
    /// Margin record margin ratio
    /// </summary>
    [JsonProperty("marginCallBar")]
    public decimal MarginCallBar { get; set; }

    /// <summary>
    /// Initial margin ratio
    /// </summary>
    [JsonProperty("normalBar")]
    public decimal NormalBar { get; set; }
}
