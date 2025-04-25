namespace Binance.Api.Wallet;

/// <summary>
/// Trade fee info
/// </summary>
public record BinanceWalletTradeFee
{
    /// <summary>
    /// The symbol this fee is for
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The fee for trades where you're the maker
    /// </summary>
    [JsonProperty("makerCommission")]
    public decimal MakerFee { get; set; }

    /// <summary>
    /// The fee for trades where you're the taker
    /// </summary>
    [JsonProperty("takerCommission")]
    public decimal TakerFee { get; set; }
}
