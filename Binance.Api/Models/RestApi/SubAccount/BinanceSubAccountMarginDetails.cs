using Binance.Api.Margin;

namespace Binance.Api.Models.RestApi.SubAccount;

/// <summary>
/// Sub account margin trade details
/// </summary>
public record BinanceSubAccountMarginDetails
{
    /// <summary>
    /// Email of the account
    /// </summary>
    public string Email { get; set; } = "";
    /// <summary>
    /// Margin level
    /// </summary>
    public decimal MarginLevel { get; set; }
    /// <summary>
    /// Total asset in btc
    /// </summary>
    public decimal TotalAssetOfBtc { get; set; }
    /// <summary>
    /// Total liability
    /// </summary>
    public decimal TotalLiabilityOfBtc { get; set; }
    /// <summary>
    /// Total net asset
    /// </summary>
    public decimal TotalNetAssetOfBtc { get; set; }
    /// <summary>
    /// Trade details
    /// </summary>
    [JsonProperty("marginTradeCoeffVo")]
    public BinanceMarginTradeCoeff MarginTradeCoeff { get; set; }
    /// <summary>
    /// Asset list
    /// </summary>
    [JsonProperty("marginUserAssetVoList")]
    public IEnumerable<BinanceMarginAccountBalance> MarginUserAssets { get; set; } = Array.Empty<BinanceMarginAccountBalance>();
}

/// <summary>
/// Margin trade detail
/// </summary>
public record BinanceMarginTradeCoeff
{
    /// <summary>
    /// Liquidation margin ratio
    /// </summary>
    public decimal ForceLiquidationBar { get; set; }
    /// <summary>
    /// Margin class margin ratio
    /// </summary>
    public decimal MarginCallBar { get; set; }
    /// <summary>
    /// Initial margin ratio
    /// </summary>
    public decimal NormalBar { get; set; }
}
