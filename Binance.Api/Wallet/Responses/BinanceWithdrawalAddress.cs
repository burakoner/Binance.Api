namespace Binance.Api.Wallet.Responses;

/// <summary>
/// Withdrawal address info
/// </summary>
public record BinanceWithdrawalAddress
{
    /// <summary>
    /// Address
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Address tag
    /// </summary>
    public string? AddressTag { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Network
    /// </summary>
    public string Network { get; set; } = string.Empty;

    /// <summary>
    /// Origin
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// Origin type
    /// </summary>
    public string OriginType { get; set; } = string.Empty;

    /// <summary>
    /// Is whitelisted
    /// </summary>
    [JsonProperty("whiteStatus")]
    public bool Whitelisted { get; set; }
}
