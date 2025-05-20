namespace Binance.Api.Staking;

/// <summary>
/// Staking Wrap Result
/// </summary>
public record BinanceEthStakingWrap
{
    /// <summary>
    /// Successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// WBETH Quantity
    /// </summary>
    [JsonProperty("wbethAmount")]
    public decimal EthQuantity { get; set; }

    /// <summary>
    /// Exchange Rate
    /// </summary>
    [JsonProperty("exchangeRate")]
    public decimal ExchangeRate { get; set; }
}
