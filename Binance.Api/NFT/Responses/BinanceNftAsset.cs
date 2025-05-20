namespace Binance.Api.NFT;

/// <summary>
/// NFT asset
/// </summary>
public record BinanceNftAsset
{
    /// <summary>
    /// NFT network
    /// </summary>
    public string Network { get; set; } = string.Empty;

    /// <summary>
    /// NFT contract address
    /// </summary>
    public string ContractAddress { get; set; } = string.Empty;

    /// <summary>
    /// NFT token id
    /// </summary>
    public string TokenId { get; set; } = string.Empty;
}
