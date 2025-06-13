namespace Binance.Api.Link;

/// <summary>
/// Sub Account Commission
/// </summary>
public record BinanceLinkSubAccountCommission
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Maker Commission
    /// </summary>
    [JsonProperty("makerCommission")]
    public decimal MakerCommission { get; set; }

    /// <summary>
    /// Taker Commission
    /// </summary>
    [JsonProperty("takerCommission")]
    public decimal TakerCommission { get; set; }

    /// <summary>
    /// Margin Maker Commission
    /// <para>If margin disabled, return -1</para>
    /// </summary>
    [JsonProperty("marginMakerCommission")]
    public decimal MarginMakerCommission { get; set; }

    /// <summary>
    /// Margin Taker Commission
    /// <para>If margin disabled, return -1</para>
    /// </summary>
    [JsonProperty("marginTakerCommission")]
    public decimal MarginTakerCommission { get; set; }
}