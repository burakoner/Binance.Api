namespace Binance.Api.Wallet;

/// <summary>
/// Asset dusts that can be converted to BNB
/// </summary>
public record BinanceEligibleDusts
{
    /// <summary>
    /// Total BTC value
    /// </summary>
    public decimal TotalTransferBTC { get; set; }
    
    /// <summary>
    /// Total BNB value
    /// </summary>
    public decimal TotalTransferBNB { get; set; }
    
    /// <summary>
    /// Commission fee
    /// </summary>
    [JsonProperty("dribbletPercentage")]
    public decimal FeePercentage { get; set; }

    /// <summary>
    /// Assets
    /// </summary>
    public IEnumerable<BinanceWalletEligibleDust> Details { get; set; } = [];
}

/// <summary>
/// Asset which can be converted
/// </summary>
public record BinanceWalletEligibleDust
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the asset
    /// </summary>
    public string AssetFullName { get; set; } = string.Empty;

    /// <summary>
    /// Amount free
    /// </summary>
    [JsonProperty("amountFree")]
    public decimal QuantityFree { get; set; }

    /// <summary>
    /// BTC value
    /// </summary>
    public decimal ToBTC { get; set; }

    /// <summary>
    /// BNB value without fee
    /// </summary>
    public decimal ToBNB { get; set; }

    /// <summary>
    /// BNB value with fee
    /// </summary>
    public decimal ToBNBOffExchange { get; set; }

    /// <summary>
    /// Fee
    /// </summary>
    [JsonProperty("exchange")]
    public decimal Fee { get; set; }
}