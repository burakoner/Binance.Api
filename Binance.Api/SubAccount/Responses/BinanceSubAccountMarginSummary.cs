namespace Binance.Api.SubAccount;

/// <summary>
/// Sub accounts margin summary
/// </summary>
public record BinanceSubAccountMarginSummary
{
    /// <summary>
    /// Total btc asset
    /// </summary>
    [JsonProperty("totalAssetOfBtc")]
    public decimal TotalAssetOfBtc { get; set; }

    /// <summary>
    /// Total liability
    /// </summary>
    [JsonProperty("totalLiabilityOfBtc")]
    public decimal TotalLiabilityOfBtc { get; set; }

    /// <summary>
    /// Total net btc
    /// </summary>
    [JsonProperty("totalNetAssetOfBtc")]
    public decimal TotalNetAssetOfBtc { get; set; }

    /// <summary>
    /// Sub account details
    /// </summary>
    [JsonProperty("subAccountList")]
    public List<BinanceSubAccountMarginInfo>  SubAccounts { get; set; } = [];
}

/// <summary>
/// Sub account margin info
/// </summary>
public record BinanceSubAccountMarginInfo
{
    /// <summary>
    /// Sub account email
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Total btc asset
    /// </summary>
    [JsonProperty("totalAssetOfBtc")]
    public decimal TotalAssetOfBtc { get; set; }

    /// <summary>
    /// Total liability
    /// </summary>
    [JsonProperty("totalLiabilityOfBtc")]
    public decimal TotalLiabilityOfBtc { get; set; }

    /// <summary>
    /// Total net btc
    /// </summary>
    [JsonProperty("totalNetAssetOfBtc")]
    public decimal TotalNetAssetOfBtc { get; set; }
}
