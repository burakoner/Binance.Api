namespace Binance.Api.Link;

/// <summary>
/// Sub Account Futures Commission
/// </summary>
public record BinanceLinkSubAccountFuturesCommission
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// USDT-Ⓜ futures commission adjustment for maker
    /// </summary>
    [JsonProperty("makerAdjustment")]
    public int MakerAdjustment { get; set; }

    /// <summary>
    /// USDT-Ⓜ futures commission adjustment for taker
    /// </summary>
    [JsonProperty("takerAdjustment")]
    public int TakerAdjustment { get; set; }

    /// <summary>
    /// USDT-Ⓜ futures commission (after adjusted) for maker
    /// </summary>
    [JsonProperty("makerCommission")]
    public decimal MakerCommission { get; set; }

    /// <summary>
    /// USDT-Ⓜ futures commission (after adjusted) for taker
    /// </summary>
    [JsonProperty("takerCommission")]
    public decimal TakerCommission { get; set; }
}