namespace Binance.Api.NFT;

/// <summary>
/// NFT deposit
/// </summary>
public record BinanceNftDeposit
{
    /// <summary>
    /// NFT network
    /// </summary>
    public string Network { get; set; } = string.Empty;

    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("txID")]
    public string? TransactionId { get; set; }

    /// <summary>
    /// NFT contract address
    /// </summary>
    [JsonProperty("contractAdrress")] // not a typo
    public string ContractAddress { get; set; } = string.Empty;

    /// <summary>
    /// NFT token id
    /// </summary>
    public string TokenId { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }
}
