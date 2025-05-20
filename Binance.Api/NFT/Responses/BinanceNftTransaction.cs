namespace Binance.Api.NFT;

/// <summary>
/// NFT transaction
/// </summary>
public record BinanceNftTransaction
{
    /// <summary>
    /// Order number, 0: purchase order, 1: sell order, 2: royalty income, 3: primary market order, 4: mint fee
    /// </summary>
    [JsonProperty("orderNo")]
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>
    /// Tokens
    /// </summary>
    public List<BinanceNftAsset> Tokens { get; set; } = [];

    /// <summary>
    /// Trade time
    /// </summary>
    public DateTime TradeTime { get; set; }

    /// <summary>
    /// Trade amount
    /// </summary>
    [JsonProperty("tradeAmount")]
    public decimal? TradeQuantity { get; set; } = null;

    /// <summary>
    /// Trade currency
    /// </summary>
    public string TradeCurrency { get; set; } = string.Empty;
}
