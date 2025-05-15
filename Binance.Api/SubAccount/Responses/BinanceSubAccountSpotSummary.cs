namespace Binance.Api.SubAccount;

/// <summary>
/// Sub accounts btc value summary
/// </summary>
public record BinanceSubAccountSpotSummary
{
    /// <summary>
    /// Total records
    /// </summary>
    [JsonProperty("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Master account total asset value
    /// </summary>
    [JsonProperty("masterAccountTotalAsset")]
    public decimal MasterAccountTotalAsset { get; set; }

    /// <summary>
    /// Sub account values
    /// </summary>
    [JsonProperty("spotSubUserAssetBtcVoList")]
    public List<BinanceSubAccountBtcValue> SubAccountsBtcValues { get; set; } = [];
}

/// <summary>
/// Sub account btc value
/// </summary>
public record BinanceSubAccountBtcValue
{
    /// <summary>
    /// Sub account email
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Sub account total asset 
    /// </summary>
    [JsonProperty("totalAsset")]
    public decimal TotalAsset { get; set; }
}
